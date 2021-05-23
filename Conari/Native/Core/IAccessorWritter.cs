﻿/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) Conari contributors https://github.com/3F/Conari/graphs/contributors
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using net.r_eg.Conari.Types;

namespace net.r_eg.Conari.Native.Core
{
    public interface IAccessorWritter<TretChain>
    {
        /// <summary>
        /// Write T type data using current possition.
        /// </summary>
        /// <typeparam name="T">
        ///     <see cref="UIntPtr"/>, <see cref="IntPtr"/>, <see cref="UInt64"/>, 
        ///     <see cref="Int64"/>, <see cref="UInt32"/>, <see cref="Int32"/>, 
        ///     <see cref="UInt16"/>, <see cref="Int16"/>, <see cref="SByte"/>, 
        ///     <see cref="byte"/>, <see cref="char"/>, <see cref="achar"/>,
        ///     <see cref="wchar"/>
        /// </typeparam>
        /// <param name="input"></param>
        TretChain write<T>(T input) where T : struct;

        /// <inheritdoc cref="write{T}(T)"/>
        TretChain write<T>(params T[] input) where T : struct;

        /// <summary>
        /// Use <see cref="write{T}(T[])"/> specified number of times.
        /// </summary>
        /// <inheritdoc cref="write{T}(T[])"/>
        TretChain repeat<T>(int count, T input) where T : struct;

        /// <summary>
        /// Write <see cref="UIntPtr"/> using current possition.
        /// </summary>
        TretChain writeUIntPtr(UIntPtr input);

        /// <summary>
        /// Write <see cref="IntPtr"/> using current possition.
        /// </summary>
        TretChain writeIntPtr(IntPtr input);

        /// <summary>
        /// Write <see cref="UInt64"/> using current possition.
        /// </summary>
        TretChain writeUInt64(UInt64 input);

        /// <summary>
        /// Write <see cref="Int64"/> using current possition.
        /// </summary>
        TretChain writeInt64(Int64 input);

        /// <summary>
        /// Write <see cref="UInt32"/> using current possition.
        /// </summary>
        TretChain writeUInt32(UInt32 input);

        /// <summary>
        /// Write <see cref="Int32"/> using current possition.
        /// </summary>
        TretChain writeInt32(Int32 input);

        /// <summary>
        /// Write <see cref="UInt16"/> using current possition.
        /// </summary>
        TretChain writeUInt16(UInt16 input);

        /// <summary>
        /// Write <see cref="Int16"/> using current possition.
        /// </summary>
        TretChain writeInt16(Int16 input);

        /// <summary>
        /// Write <see cref="byte"/> using current possition.
        /// </summary>
        TretChain writeByte(byte input);

        /// <summary>
        /// Write as a 8 bit character using current possition.
        /// </summary>
        TretChain writeChar(char input);

        /// <summary>
        /// Write as a 16 bit character using current possition.
        /// </summary>
        TretChain writeWChar(char input);

        /// <summary>
        /// Write either as a 16 bit or a 8 bit character 
        /// depending on the configuration (eg. <see cref="IAccessor.set(CharType)"/> etc)
        /// at current possition.
        /// </summary>
        TretChain writeTChar(char input);

        /// <summary>
        /// Write either as a 16 bit or a 8 bit character 
        /// depending on specified type at current possition.
        /// </summary>
        /// <remarks>It will ignore configurations such as <see cref="IAccessor.set(CharType)"/> etc.</remarks>
        /// <param name="input"></param>
        /// <param name="enc"></param>
        TretChain writeChar(char input, CharType enc);

        /// <summary>
        /// Write <see cref="sbyte"/> using current possition.
        /// </summary>
        TretChain writeSByte(sbyte input);
    }
}
