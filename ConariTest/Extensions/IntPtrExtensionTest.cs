﻿/*!
 * Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) Conari contributors https://github.com/3F/Conari/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/Conari
*/

using System;
using System.Linq;
using System.Text;
using net.r_eg.Conari.Extension;
using net.r_eg.Conari.Types;
using Xunit;

namespace ConariTest.Extensions
{
    public class IntPtrExtensionTest
    {
        [Fact]
        public void GetStringLengthTest1()
        {
            const int BSIZE = 1;
            string managed  = " my string 123 ";

            using(var uns = new NativeString<CharPtr>(managed)) {
                IntPtr ptr = uns;
                Assert.Equal(managed.Length * BSIZE, ptr.GetStringLength(BSIZE));
            }
        }

        [Fact]
        public void GetStringLengthTest2()
        {
            const int BSIZE = 2;
            string managed  = " my string 123 ";

            using(var uns = new NativeString<WCharPtr>(managed)) {
                IntPtr ptr = uns;
                Assert.Equal(managed.Length * BSIZE, ptr.GetStringLength(BSIZE));
            }
        }

#pragma warning disable CS0618 // Type or member is obsolete
        [Fact]
        public void GetStringLengthTest3()
        {
            const int BSIZE = 2;
            string managed  = " my string 123 ";

            using(var uns = new UnmanagedString(managed, UnmanagedString.SType.BSTR)) {
                IntPtr ptr = uns;
                Assert.Equal(managed.Length * BSIZE, ptr.GetStringLength(BSIZE));
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete

        [Fact]
        public void GetStringBytesTest1()
        {
            string managed = " mystring ";

            using(var uns = new NativeString<CharPtr>(managed)) {
                IntPtr ptr = uns;

                Assert.Empty(ptr.GetStringBytes(-1));
                Assert.Empty(ptr.GetStringBytes(0));
                Assert.Equal(5, ptr.GetStringBytes(5).Length);

                var a = Encoding.UTF8.GetBytes(managed).Take(3).ToArray();
                var b = ptr.GetStringBytes(3);

                Assert.Equal(a[0], b[0]);
                Assert.Equal(a[1], b[1]);
                Assert.Equal(a[2], b[2]);
            }
        }
    }
}
