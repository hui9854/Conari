﻿/*!
 * Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) Conari contributors https://github.com/3F/Conari/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/Conari
*/

using System;
using net.r_eg.Conari;
using net.r_eg.Conari.Aliases;
using net.r_eg.Conari.Core;
using net.r_eg.Conari.Types;
using Xunit;

namespace ConariTest
{
    using static net.r_eg.Conari.Static.Members;
    using static _svc.TestHelper;

    /// <summary>
    /// ConariX related tests
    /// </summary>
    public class XAliasesTest
    {
        private readonly IConfig gCfgUnlib = new Config(UNLIB_DLL, true);

        [Fact]
        public void aliasTest1()
        {
            // bool net::r_eg::Conari::UnLib::API::getD_True(void)
            // ?getD_True@common@UnLib@Conari@r_eg@net@@YA_NXZ

            using(dynamic l = new ConariX(UNLIB_DLL, true))
            {
                l.Aliases["getD_True"] = new ProcAlias("?getD_True@common@UnLib@Conari@r_eg@net@@YA_NXZ");

                Assert.Equal(true, l.getD_True<bool>());
                Assert.True(l.bind<Func<bool>>("getD_True")());
                Assert.Equal(true, l.bind(Dynamic.GetMethodInfo(typeof(bool)), "getD_True")
                                        .dynamic
                                        .Invoke(null, Array.Empty<object>()));
            }
        }

        [Fact]
        public void aliasTest2()
        {
            // unsigned short net::r_eg::Conari::UnLib::API::getD_Seven(void)
            // ?getD_Seven@common@UnLib@Conari@r_eg@net@@YAGXZ

            using(dynamic l = new ConariX(gCfgUnlib))
            {
                l.Aliases["getD_Seven"] = new ProcAlias("?getD_Seven@common@UnLib@Conari@r_eg@net@@YAGXZ");

                Assert.Equal(7, l.getD_Seven<ushort>());
                Assert.Equal(7, l.bind<Func<ushort>>("getD_Seven")());
                Assert.Equal((ushort)7, l.bind(Dynamic.GetMethodInfo(typeof(ushort)), "getD_Seven")
                                                .dynamic
                                                .Invoke(null, Array.Empty<object>()));
            }
        }

        [Fact]
        public void aliasTest3()
        {
            // char const * net::r_eg::Conari::UnLib::API::getD_HelloWorld(void)
            // x86: ?getD_HelloWorld@common@UnLib@Conari@r_eg@net@@YAPBDXZ
            // x64: ?getD_HelloWorld@common@UnLib@Conari@r_eg@net@@YAPEBDXZ
            using(dynamic l = new ConariX(gCfgUnlib))
            {
                string xfun;
                if(Is64bit) {
                    xfun = "?getD_HelloWorld@common@UnLib@Conari@r_eg@net@@YAPEBDXZ";
                }
                else {
                    xfun = "?getD_HelloWorld@common@UnLib@Conari@r_eg@net@@YAPBDXZ";
                }

                string exp = "Hello World !";
                l.Aliases["getD_HelloWorld"] = new ProcAlias(xfun);

                Assert.Equal(exp, l.getD_HelloWorld<CharPtr>());
                Assert.Equal(exp, l.bind<Func<CharPtr>>("getD_HelloWorld")());

                var dyn = l.bind(Dynamic.GetMethodInfo(typeof(CharPtr)), "getD_HelloWorld");
                Assert.Equal(exp, (CharPtr)dyn.dynamic.Invoke(null, Array.Empty<object>()));
            }
        }

        [Fact]
        public void aliasTest4()
        {
            using(dynamic l = new ConariX(gCfgUnlib))
            {
                UInt32 expected = 0x00001CE8;

                l.Aliases["addr"]
                    = l.Aliases["_"]
                    = "ADDR_SPEC";

                Assert.Equal(expected, (UInt32)l.V.addr);
                Assert.Equal(expected, l.V._<UInt32>());

                Assert.Equal(expected, (UInt32)l.ExVar.DLR.addr);
                Assert.Equal(expected, l.ExVar.DLR._<UInt32>());

                Assert.Equal(expected, (UInt32)l.ExVar.get("addr"));
                Assert.Equal(expected, l.ExVar.get<UInt32>("addr"));
                Assert.Equal(expected, (UInt32)l.ExVar.getVar("_"));
                Assert.Equal(expected, l.ExVar.getVar<UInt32>("_"));

                Assert.Equal(expected, l.ExVar.getField<UInt32>("addr").value);
                Assert.Equal(expected, l.ExVar.getField(typeof(UInt32), "_").value);
            }
        }

        [Fact]
        public void aliasPrefixTest1()
        {
            using(dynamic l = new ConariX(UNLIB_DLL, true, "apiprefix_"))
            {
                bool expected = false;

                l.Aliases["GF"] = l.Aliases["apiprefix_GF"] = "GFlag";

                Assert.Equal(expected, l.V.GF<bool>());
                Assert.Equal(expected, l.ExVar.DLR.GF<bool>());

                Assert.Equal(expected, l.ExVar.get<bool>("GF"));
                Assert.Equal(expected, l.ExVar.getVar<bool>("apiprefix_GF"));

                Assert.Equal(expected, l.ExVar.getField<bool>("apiprefix_GF").value);
                Assert.Equal(expected, l.ExVar.getField(typeof(bool), "apiprefix_GF").value);
            }
        }

        [Fact]
        public void aliasPrefixTest2()
        {
            using(dynamic l = new ConariX(UNLIB_DLL, true, "apiprefix_"))
            {
                bool expected = false;

                var pa = new ProcAlias("apiprefix_GFlag", new AliasCfg() { NoPrefixR = true });
                l.Aliases["GF"] = l.Aliases["apiprefix_GF"] = pa;

                Assert.Equal(expected, l.V.GF<bool>());
                Assert.Equal(expected, l.ExVar.DLR.GF<bool>());

                Assert.Equal(expected, l.ExVar.get<bool>("GF"));
                Assert.Equal(expected, l.ExVar.getVar<bool>("apiprefix_GF"));

                Assert.Equal(expected, l.ExVar.getField<bool>("apiprefix_GF").value);
                Assert.Equal(expected, l.ExVar.getField(typeof(bool), "apiprefix_GF").value);
            }
        }

        [Fact]
        public void aliasPrefixTest3()
        {
            using(dynamic l = new ConariX(UNLIB_DLL, true, "apiprefix_"))
            {
                bool expected = false;

                var pa = new ProcAlias("apiprefix_GFlag", new AliasCfg() { NoPrefixR = true });
                l.Aliases["GF"] = l.Aliases["apiprefix_GF"] = pa;
                
                Assert.Equal(expected, l.ExVar.get<bool>("apiprefix_GF"));
                Assert.Equal(expected, l.ExVar.getVar<bool>("GF"));
                Assert.Equal(expected, l.ExVar.getField<bool>("GF").value);
                Assert.Equal(expected, l.ExVar.getField(typeof(bool), "GF").value);
            }
        }

        [Fact]
        public void aliasPrefixTest4()
        {
            using(dynamic l = new ConariX(gCfgUnlib))
            {
                l.Prefix = "apiprefix_";

                string xfun;
                if(Is64bit) {
                    xfun = "?getD_HelloWorld@common@UnLib@Conari@r_eg@net@@YAPEBDXZ";
                }
                else {
                    xfun = "?getD_HelloWorld@common@UnLib@Conari@r_eg@net@@YAPBDXZ";
                }

                l.Aliases["HelloWorld"] = new ProcAlias(
                    xfun, 
                    new AliasCfg() { NoPrefixR = true }
                );

                string exp = "Hello World !";

                Assert.Equal(exp, l.HelloWorld<CharPtr>());

                l.Prefix = "";
                Assert.Equal(exp, l.HelloWorld<CharPtr>());
            }
        }

        [Fact]
        public void aliasPrefixTest5()
        {
            using(dynamic l = new ConariX(gCfgUnlib))
            {
                var pa = new ProcAlias("apiprefix_GFlag", new AliasCfg() { NoPrefixR = true });
                l.Aliases["one"] = l.Aliases["two"] = pa;

                bool exp = false;

                l.Prefix = "apiprefix_";
                Assert.Equal(exp, l.V.one<bool>());
                Assert.Equal(exp, l.ExVar.DLR.one<bool>());

                l.Prefix = "";
                Assert.Equal(exp, l.V.one<bool>());
                Assert.Equal(exp, l.ExVar.DLR.one<bool>());
            }
        }

        [Fact]
        public void aliasPrefixTest6()
        {
            using(dynamic l = new ConariX(gCfgUnlib))
            {
                l.Aliases["GMN1"] = new ProcAlias(
                    "GetMagicNum", 
                    new AliasCfg() { NoPrefixR = true }
                );

                l.Aliases["GMN2"] = new ProcAlias(
                    "GetMagicNum",
                    new AliasCfg() { NoPrefixR = false }
                );

                l.Prefix = "apiprefix_";
                Assert.Equal(-1, l.GMN1<int>());
                Assert.Equal(4, l.GMN2<int>());

                l.Prefix = "";
                Assert.Equal(-1, l.GMN1<int>());
                Assert.Equal(-1, l.GMN2<int>());
            }
        }

        [Fact]
        public void multiAliasTest1()
        {
            using(dynamic l = new ConariX(gCfgUnlib))
            {
                l.Aliases["getD_True"]  = "?getD_True@common@UnLib@Conari@r_eg@net@@YA_NXZ";
                l.Aliases["getFlag"]    = l.Aliases["getD_True"];

                Assert.Equal(true, l.getD_True<bool>());
                Assert.Equal(true, l.getFlag<bool>());
                Assert.True(l.bind<Func<bool>>("getFlag")());
                Assert.Equal(true, l.bind(Dynamic.GetMethodInfo(typeof(bool)), "getFlag")
                                        .dynamic
                                        .Invoke(null, Array.Empty<object>()));
            }
        }

        [Fact]
        public void multiAliasTest2()
        {
            using(dynamic l = new ConariX(gCfgUnlib))
            {
                l.Aliases["d"]  = "?getD_True@common@UnLib@Conari@r_eg@net@@YA_NXZ";
                l.Aliases["a"] = l.Aliases["b"] = l.Aliases["c"] = l.Aliases["d"];

                Assert.Equal(true, l.a<bool>());
                Assert.Equal(true, l.b<bool>());
                Assert.Equal(true, l.c<bool>());
                Assert.Equal(true, l.d<bool>());
            }
        }
    }
}
