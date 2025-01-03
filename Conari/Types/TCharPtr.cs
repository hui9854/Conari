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
using net.r_eg.Conari.Resources;

namespace net.r_eg.Conari.Types
{
    /// <summary>
    /// Runtime-based type. <see cref="CharPtr"/> or <see cref="WCharPtr"/>.
    /// </summary>
    [DebuggerDisplay("{dbgInfo()}")]
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct TCharPtr: IPtr, ISerializable
    {
        [FieldOffset(0)]
        private readonly WCharPtr wdata;

        [FieldOffset(0)]
        private readonly CharPtr data;

        public static readonly TCharPtr Null;

        private static bool? _unicode;

        public static bool Unicode
        {
            get
            {
                if(_unicode == null) _unicode = false;
                return _unicode == true;
            }
            set
            {
                if(_unicode != null) throw new NotSupportedException(Msg.configure_once);
                _unicode = value;
            }
        }

        public IntPtr AddressPtr => this;

        /// <inheritdoc cref="CharPtr.Length"/>
        public int Length => Unicode ? wdata.Length : data.Length;

        /// <inheritdoc cref="CharPtr.StrLength"/>
        public int StrLength => Unicode ? wdata.StrLength : data.StrLength;

        /// <inheritdoc cref="CharPtr.Raw"/>
        public byte[] Raw => Unicode ? wdata.Raw : data.Raw;

        [NativeType]
        public static implicit operator IntPtr(TCharPtr v) => Unicode ? v.wdata : v.data;
        public static implicit operator string(TCharPtr v) => Unicode ? v.wdata : v.data;

        public static implicit operator TCharPtr(IntPtr ptr) => new(ptr);
        public static explicit operator TCharPtr(Int64 v) => new((IntPtr)v);
        public static explicit operator TCharPtr(Int32 v) => new((IntPtr)v);

        public static implicit operator TCharPtr(WCharPtr ptr)
        {
            FailIfAnsi();
            return new(ptr);
        }

        public static implicit operator TCharPtr(CharPtr ptr)
        {
            FailIfUnicode();
            return new(ptr);
        }

        public static implicit operator WCharPtr(TCharPtr v)
        {
            FailIfAnsi();
            return v.wdata;
        }

        public static implicit operator CharPtr(TCharPtr v)
        {
            FailIfUnicode();
            return v.data;
        }

        public static bool operator ==(TCharPtr a, TCharPtr b) => a.Equals(b);

        public static bool operator !=(TCharPtr a, TCharPtr b) => !(a == b);

        public override bool Equals(object obj)
        {
            if(obj is null || obj is not TCharPtr b) {
                return false;
            }

            return wdata == b.wdata
                    && data == b.data;
        }

        public override int GetHashCode() => 0.CalculateHashCode(wdata, data);

        public override string ToString() => this;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if(info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(wdata), wdata);
            info.AddValue(nameof(data), data);
        }

        public TCharPtr(WCharPtr data)
            : this()
        {
            wdata = data;
        }

        public TCharPtr(CharPtr data)
            : this()
        {
            this.data = data;
        }

        public TCharPtr(IntPtr pointer)
            : this()
        {
            data = pointer; //+wdata at the same 0 position
        }

        private static void FailIfUnicode()
        {
            if(Unicode) throw new NotSupportedException();
        }

        private static void FailIfAnsi()
        {
            if(!Unicode) throw new NotSupportedException();
        }

        #region unit-tests

        /// <remarks>Synchronize context before any use!</remarks>
        internal static bool? __Unicode { get => _unicode; set => _unicode = value; }

        #endregion

        #region DebuggerDisplay

        private string dbgInfo()
        {
            IntPtr ptr = data;
            return ptr == IntPtr.Zero 
                    ? "<nullptr>"
                    : $"{(string)this}    [ An {StrLength} of a {(Unicode ? "16" : "8")}-bit characters at 0x{ptr.ToString("x")} ]";
        }

        #endregion
    }
}
