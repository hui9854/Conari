﻿/*!
 * Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) Conari contributors https://github.com/3F/Conari/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/Conari
*/

using System;
using System.Runtime.InteropServices;
using net.r_eg.Conari;
using net.r_eg.Conari.Accessors.WinAPI;
using Xunit;

#if !NETCORE
using ConariTest._svc.Extensions;
#endif

namespace ConariTest.Accessors.WinAPI
{
    public class User32Test
    {
        [Fact]
        public void ctorTest1()
        {
            using(dynamic user32 = new User32())
            {
                Assert.True(user32.Library.IsActive);
                Assert.NotEqual(IntPtr.Zero, user32.Library.handle);
                Assert.False(user32.Library.isolated);

                Assert.NotNull(user32.Library.resolved);
                Assert.True(user32.Library.resolved.value);

                string module = (string)user32.Library.module;
                Assert.NotNull(module);
                Assert.True(module.Contains("user32", StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void ctorTest2()
        {
            using(dynamic user32 = new User32(new Config() 
            { 
                Module = "CustomModule.dll"
            }))
            {
                Assert.Equal(CallingConvention.Winapi, user32.Convention);

                Assert.True(user32.Library.IsActive);
                Assert.NotEqual(IntPtr.Zero, user32.Library.handle);
                Assert.False(user32.Library.isolated);

                Assert.NotNull(user32.Library.resolved);
                Assert.True(user32.Library.resolved.value);

                string module = (string)user32.Library.module;
                Assert.NotNull(module);
                Assert.True(module.Contains("user32", StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void ctorTest3()
        {
            using(dynamic user32 = new User32((IConfig)null))
            {
                Assert.True(user32.Library.IsActive);
                Assert.NotEqual(IntPtr.Zero, user32.Library.handle);
                Assert.False(user32.Library.isolated);

                Assert.NotNull(user32.Library.resolved);
                Assert.True(user32.Library.resolved.value);

                string module = (string)user32.Library.module;
                Assert.NotNull(module);
                Assert.True(module.Contains("user32", StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void convTest1()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                using(dynamic user32 = new User32())
                {
                    user32.Convention = CallingConvention.Cdecl;
                }
            });
        }
    }
}
