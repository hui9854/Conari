﻿/*!
 * Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) Conari contributors https://github.com/3F/Conari/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/Conari
*/

using System;
using net.r_eg.Conari.Types;
using Xunit;

namespace ConariTest.Types
{
    public class CharsTest
    {
        [Fact]
        public void wcharTest1()
        {
            string str1 = "あいうえお ;";
            string str2 = "あいうえお .";

            using NativeString<WCharPtr> uns = new(str1);

            WCharPtr ch = uns;
            Assert.Equal("あいうえお ;", ch);
            Assert.Equal(7, ch.StrLength);
            Assert.Equal(14, ch.Length);

            using NativeString<WCharPtr> uns2 = new(str1);
            WCharPtr ch2 = uns2;
            WCharPtr ch3 = uns;
            Assert.Equal(ch2, ch);
            Assert.Equal(ch3, ch);

            using NativeString<WCharPtr> uns3 = new(str2);
            WCharPtr ch4 = uns3;
            Assert.NotEqual(ch4, ch);
        }

        [Fact]
        public void wcharCtorTest1()
        {
            using NativeString<WCharPtr> uns = new(String.Empty);

            WCharPtr ch = uns;
            Assert.NotEqual(WCharPtr.Null, ch);
            Assert.Equal(WCharPtr.Null, new WCharPtr());
            Assert.Equal(0, ch.StrLength);
            Assert.Equal(0, ch.Length);

            Assert.Throws<ArgumentOutOfRangeException>(() => (WCharPtr)IntPtr.Zero);
        }

        [Fact]
        public void wcharCtorTest2()
        {
            using NativeString<WCharPtr> uns = new(" ");

            WCharPtr ch = uns;
            Assert.NotEqual(WCharPtr.Null, ch);
            Assert.Equal(1, ch.StrLength);
            Assert.Equal(2, ch.Length);
        }

        [Fact]
        public void charTest1()
        {
            string str1 = "test of chars ;";
            string str2 = "test of chars .";

            using NativeString<CharPtr> uns = new(str1);

            CharPtr ch = uns;
            Assert.Equal("test of chars ;", ch);
            Assert.Equal(15, ch.StrLength);
            Assert.Equal(15, ch.Length);

            using NativeString<CharPtr> uns2 = new(str1);
            CharPtr ch2 = uns2;
            CharPtr ch3 = uns;
            Assert.Equal(ch2, ch);
            Assert.Equal(ch3, ch);

            using NativeString<CharPtr> uns3 = new(str2);
            CharPtr ch4 = uns3;
            Assert.NotEqual(ch4, ch);
        }

        [Fact]
        public void charCtorTest1()
        {
            using NativeString<CharPtr> uns = new(string.Empty);

            CharPtr ch = uns;
            Assert.NotEqual(CharPtr.Null, ch);
            Assert.Equal(CharPtr.Null, new CharPtr());
            Assert.Equal(0, ch.StrLength);
            Assert.Equal(0, ch.Length);

            Assert.Throws<ArgumentOutOfRangeException>(() => (CharPtr)IntPtr.Zero);
        }

        [Fact]
        public void charCtorTest2()
        {
            using NativeString<CharPtr> uns = new(" ");

            CharPtr ch = uns;
            Assert.NotEqual(CharPtr.Null, ch);
            Assert.Equal(1, ch.StrLength);
            Assert.Equal(1, ch.Length);
        }

        [Fact]
        public void diffCharsTest1()
        {
            Assert.True(WCharPtr.Null == new WCharPtr());
            Assert.False(WCharPtr.Null != new WCharPtr());
            Assert.False(WCharPtr.Null == CharPtr.Null);
            Assert.True(WCharPtr.Null != CharPtr.Null);

            Assert.True(CharPtr.Null == new CharPtr());
            Assert.False(CharPtr.Null != new CharPtr());
            Assert.False(CharPtr.Null == WCharPtr.Null);
            Assert.True(CharPtr.Null != WCharPtr.Null);

            Assert.True(TCharPtr.Null == new TCharPtr());
            Assert.False(TCharPtr.Null != new TCharPtr());
        }
    }
}
