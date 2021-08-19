using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
{
    class Memory
    {
        byte[] values = new byte[65536]; // 0-65535 Adresów

        public Memory()
        {
            for (int i = 0; i < 65535; i++)
            {
                values[i] = 0;
            }
        }

        public void setByte(ushort address, byte value)
        {
            values[address] = value;
        }

        public byte getByte(ushort address)
        {
            return values[address];
        }

        public void setWord(ushort address, ushort value)
        {
            if (address < 65535)
            {
                values[address + 1] = (byte)(value >> 8);  // 11110000 | 00001111
                values[address] = (byte)(value & 255);
            }

        }

        public ushort getWord(ushort address)
        {
            return BitConverter.ToUInt16(new byte[] { values[address], values[address + 1] });
        }


    }
}
