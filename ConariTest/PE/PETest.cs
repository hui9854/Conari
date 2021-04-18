﻿using System;
using System.Collections.Generic;
using System.Linq;
using net.r_eg.Conari;
using net.r_eg.Conari.PE;
using net.r_eg.Conari.PE.WinNT;
using Xunit;

namespace ConariTest
{
    using static _svc.TestHelper;

    public class PETest
    {
        public static IEnumerable<object[]> GetPE64obj()
        {
            IPE pe = new PEFile(RXW_X64);
            yield return new object[] { pe, pe as IDisposable };

            ConariL l = new(RXW_X64);
            yield return new object[] { l.PE, l };

            ConariX x = new(RXW_X64);
            yield return new object[] { x.PE, x };
        }

        [Theory]
        [MemberData(nameof(GetPE64obj))]
        public void pe64Theory1(IPE pe, IDisposable obj)
        {
            Assert.Equal(RXW_X64, pe.FileName);
            Assert.Equal(Magic.PE64, pe.Magic);
            Assert.Equal(MachineTypes.IMAGE_FILE_MACHINE_AMD64, pe.Machine);
            Assert.Equal
            (
                Characteristics.IMAGE_FILE_EXECUTABLE_IMAGE | Characteristics.IMAGE_FILE_LARGE_ADDRESS_AWARE | Characteristics.IMAGE_FILE_DLL,
                pe.Characteristics
            );
            obj?.Dispose();
        }

        [Theory]
        [MemberData(nameof(GetPE64obj))]
        public void pe64Theory2(IPE pe, IDisposable obj)
        {
            Assert.NotNull(pe.Export);
            Assert.NotEmpty(pe.Export.Names);
            Assert.Equal(pe.Export.NameOrdinals.Count(), pe.Export.Names.Count());
            Assert.NotEmpty(pe.Export.Addresses);
            Assert.Equal(pe.ExportedProcNames.Count(), pe.Export.Names.Count());
            Assert.True(pe.Sections.Length > 1);
            Assert.True(pe.DExport.AddressOfFunctions != 0);
            Assert.True(pe.DExport.AddressOfNameOrdinals != 0);
            Assert.True(pe.DExport.AddressOfNames != 0);
            obj?.Dispose();
        }

        public static IEnumerable<object[]> GetPE32obj()
        {
            IPE pe = new PEFile(RXW_X32);
            yield return new object[] { pe, pe as IDisposable };

#if TEST_MEM_LOAD_X32
            ConariL l = new(RXW_X32);
            yield return new object[] { l.PE, l };

            ConariX x = new(RXW_X32);
            yield return new object[] { x.PE, x };
#endif
        }

        [Theory]
        [MemberData(nameof(GetPE32obj))]
        public void pe32Theory1(IPE pe, IDisposable obj)
        {
            Assert.Equal(RXW_X32, pe.FileName);
            Assert.Equal(Magic.PE32, pe.Magic);
            Assert.Equal(MachineTypes.IMAGE_FILE_MACHINE_I386, pe.Machine);
            Assert.Equal
            (
                Characteristics.IMAGE_FILE_EXECUTABLE_IMAGE | Characteristics.IMAGE_FILE_32BIT_MACHINE | Characteristics.IMAGE_FILE_DLL,
                pe.Characteristics
            );
            obj?.Dispose();
        }

        [Theory]
        [MemberData(nameof(GetPE32obj))]
        public void pe32Theory2(IPE pe, IDisposable obj)
        {
            Assert.NotNull(pe.Export);
            Assert.NotEmpty(pe.Export.Names);
            Assert.Equal(pe.Export.NameOrdinals.Count(), pe.Export.Names.Count());
            Assert.NotEmpty(pe.Export.Addresses);
            Assert.Equal(pe.ExportedProcNames.Count(), pe.Export.Names.Count());
            Assert.True(pe.Sections.Length > 1);
            Assert.True(pe.DExport.AddressOfFunctions != 0);
            Assert.True(pe.DExport.AddressOfNameOrdinals != 0);
            Assert.True(pe.DExport.AddressOfNames != 0);
            obj?.Dispose();
        }
    }
}