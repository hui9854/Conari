﻿/*!
 * Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) Conari contributors https://github.com/3F/Conari/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/Conari
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using net.r_eg.Conari.Extension;
using net.r_eg.Conari.Native;
using net.r_eg.Conari.Native.Core;
using net.r_eg.Conari.Resources;
using net.r_eg.Conari.Types.Structs;

namespace net.r_eg.Conari.Types
{
    /// <summary>
    /// Fully automatic way of working with structures without declarations 
    /// using <see cref="NativeData"/> chains.
    /// </summary>
    /// <remarks>Alternatively use <see cref="NativeStruct{T}"/> to specify some available declarations.</remarks>
    public class NativeStruct: NativeStruct<NeutralStruct>, IMarshalableGeneric, ISerializable, IDisposable
    {
        protected readonly NativeData chain;

        /// <summary>
        /// Make a new runtime structure based on <see cref="NativeData"/>.
        /// </summary>
        public static NativeData Make => new(new Memory(IntPtr.Zero));

        /// <summary>
        /// Accessing data in runtime structure.
        /// </summary>
        /// <remarks>Only if structure was based on <see cref="NativeData"/>. Otherwise construct a new <see cref="NativeData"/> manually.</remarks>
        /// <exception cref="NotSupportedException"></exception>
        public dynamic Access => chain?.build() ?? throw new NotSupportedException();

        /// <summary>
        /// Create a new <see cref="NeutralStruct"/> to construct data using <see cref="NativeData"/> chains.
        /// </summary>
        /// <param name="buffer">Reserved size in bytes for data in memory.</param>
        public NativeStruct(int buffer = 0x40)
        {
            Size    = Math.Max(0, buffer);
            pointer = Marshal.AllocHGlobal(Size);
            Owner   = true;
        }

        /// <param name="def">Prepared chain to final construction.</param>
        internal NativeStruct(NativeData def)
            : this(def?.Size ?? throw new ArgumentNullException(nameof(def)))
        {
            if(def.Access.CurrentPtr == VPtr.Zero)
            {
                def.Access.CurrentPtr = pointer;
                def.Access.shiftRegionPtr();
            }
            chain = def;
        }
    }

    /// <summary>
    /// Semi-automatic way of working with structures
    /// using CLR types declarations.
    /// </summary>
    /// <remarks>Alternatively use <see cref="NativeStruct"/> as a completely automated implementation.</remarks>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{DbgInfo}")]
    [Serializable]
    public class NativeStruct<T>: IMarshalableGeneric, ISerializable, IDisposable
        where T : struct
    {
        protected IntPtr pointer;

        private T? readed;

        public Type MarshalableType { get; } = typeof(T);

        public IntPtr AddressPtr => this;

        /// <summary>
        /// Construct a new structure data using <see cref="NativeData"/> chains.
        /// </summary>
        /// <remarks>Alternatively use <see cref="NativeStruct.Make"/> for more automation.</remarks>
        public NativeData Native => pointer.Native();

        /// <summary>
        /// Accessing data. <br/>
        /// Only if structure was NOT based on <see cref="NeutralStruct"/>.
        /// </summary>
        /// <remarks>Use <see cref="read()"/> to update local values. Use <see cref="NativeStruct.Access"/> for more automation.</remarks>
        public T Data => (readed ?? read().readed).Value;

        /// <summary>
        /// Produced native data size in bytes.
        /// </summary>
        public int Size { get; protected set; }

        /// <summary>
        /// Who is the owner. True indicates its own allocating.
        /// </summary>
        public bool Owner { get; protected set; }

        protected static bool IsNeutralStruct { get; } = typeof(T) == typeof(NeutralStruct);

        [NativeType]
        public static implicit operator IntPtr(NativeStruct<T> v) => v.pointer;

        public static implicit operator T(NativeStruct<T> v) => v.Data;

        public static bool operator ==(NativeStruct<T> a, T b) => a.Equals(b);

        public static bool operator !=(NativeStruct<T> a, T b) => !(a == b);

        public static bool operator ==(T a, NativeStruct<T> b) => b.Equals(a);

        public static bool operator !=(T a, NativeStruct<T> b) => !(b == a);

        public static bool operator ==(NativeStruct<T> a, NativeStruct<T> b) => a.Equals(b);

        public static bool operator !=(NativeStruct<T> a, NativeStruct<T> b) => !(a == b);

        public override bool Equals(object obj)
        {
            if(obj is null) return false;
            if(!IsNeutralStruct && obj is T s) return Data.Equals(s);

            if(obj is not NativeStruct<T> b) return false;

            // NOTE: actual values can be different for pointer == b.pointer

            return !IsNeutralStruct && Data.Equals(b.Data);
        }

        public override int GetHashCode()
        {
            return 0.CalculateHashCode
            (
                pointer,
                readed/*<-Data*/,
                Size,
                Owner
            );
        }

        public override string ToString()
        {
            if(pointer == IntPtr.Zero) return null;
            return Data.ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if(info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(pointer), pointer);
            info.AddValue(nameof(readed), readed);
            info.AddValue(nameof(Owner), Owner);
        }

        /// <inheritdoc cref="read(out T)"/>
        public NativeStruct<T> read()
        {
            if(!IsNeutralStruct)
            {
                releaseData();
                readed = (T)Marshal.PtrToStructure(pointer, typeof(T));
                return this;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Read new data from memory.
        /// Only if structure was NOT based on <see cref="NeutralStruct"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>
        /// Alternatively use <see cref="Data"/> to access data. 
        /// Use <see cref="NativeStruct.Access"/> for more automation.
        /// </remarks>
        public NativeStruct<T> read(out T data)
        {
            read();
            data = readed.Value;
            return this;
        }

        /// <summary>
        /// Create a new native unmanaged T structure without initialization.
        /// </summary>
        /// <remarks>Structure data will not be initialized! To initialize, use <see cref="NativeStruct{T}(T)"/> instead.</remarks>
        public NativeStruct()
        {
            if(!IsNeutralStruct)
            {
                Size    = Marshal.SizeOf(typeof(T));
                pointer = Marshal.AllocHGlobal(Size);
                Owner   = true;
            }
        }

        /// <summary>
        /// Create a new native unmanaged T structure with initialized data using dotnet managed environment.
        /// </summary>
        /// <param name="input">Input managed data.</param>
        public NativeStruct(T input)
            : this()
        {
            if(IsNeutralStruct) throw new NotSupportedException();
            Marshal.StructureToPtr(input, pointer, fDeleteOld: false);
        }

        /// <summary>
        /// New <see cref="NativeStruct{T}"/> instance to work with already allocated unmanaged T structure.
        /// </summary>
        /// <param name="pointer">Valid pointer to structure.</param>
        public NativeStruct(IntPtr pointer)
        {
            if(pointer == IntPtr.Zero) throw new ArgumentOutOfRangeException(Msg.invalid_pointer);
            this.pointer = pointer;
        }

        protected void releaseData()
        {
            if(pointer == IntPtr.Zero) throw new ArgumentOutOfRangeException(Msg.invalid_pointer);
            if(readed != null) Marshal.DestroyStructure(pointer, typeof(T));
        }

        protected void release(bool disposing = true)
        {
            releaseData();
            if(Owner)
            {
                Marshal.FreeHGlobal(pointer);
                Owner = false;
            }

            if(disposing)
            {
                pointer = IntPtr.Zero;
                readed  = null;
            }
        }
        
        #region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(!disposed)
            {
                release(disposing);
                disposed = true;
            }
        }

        ~NativeStruct() => Dispose(false);

        #endregion

        #region DebuggerDisplay

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DbgInfo
            => pointer == IntPtr.Zero 
                ? "<nullptr>"
                : $"[ {Size} bytes at 0x{pointer.ToString("x")} ]";

        #endregion
    }
}
