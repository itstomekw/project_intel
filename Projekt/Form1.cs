using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt
{
    public partial class Form1 : Form
    {

        Dictionary<string, Register16> GeneralPurposeRegisters = new()
        {
            { "AX", new Register16() },
            { "BX", new Register16() },
            { "CX", new Register16() },
            { "DX", new Register16() },
            { "BP", new Register16() },
            { "DI", new Register16() },
            { "SI", new Register16() },
            { "SP", new Register16() },
            { "DISP", new Register16() },
        };

        Memory memory = new();
        Memory stack = new();
        string instruction = null;

        public Form1()
        {
            InitializeComponent();

        }

        //Instrukcje

        //MOV
        void MOV(Register16 src, Register16 dest)
        {
            dest.value = src.value;
        }

        void MOV(Register16 src, ushort address)
        {
            memory.setWord(address, src.value);
        }

        void MOV(ushort address, Register16 dest)
        {
            dest.value = memory.getWord(address);
        }

        //XCHG
        void XCHG(Register16 src, Register16 dest)
        {
            ushort a = dest.value;
            dest.value = src.value;
            src.value = a;
        }

        void XCHG(Register16 src, ushort address)
        {
            ushort a = memory.getWord(address);
            memory.setWord(address, src.value);
            src.value = a;
        }
        void XCHG(ushort address, Register16 dest)
        {
            ushort a = memory.getWord(address);
            memory.setWord(address, dest.value);
            dest.value = a;
        }

        void PUSH(Register16 src)
        {
            stack.setWord(GeneralPurposeRegisters["SP"].value, src.value);
            GeneralPurposeRegisters["SP"].value += 2;
        }
        void PUSH(ushort src)
        {
            stack.setWord(GeneralPurposeRegisters["SP"].value, memory.getWord(src));
            GeneralPurposeRegisters["SP"].value += 2;
        }
        void POP(Register16 src)
        {
            src.value = stack.getWord((ushort)(GeneralPurposeRegisters["SP"].value - 2));
            GeneralPurposeRegisters["SP"].value -= 2;
        }
        void POP(ushort src)
        {
            memory.setWord(src, stack.getWord((ushort)(GeneralPurposeRegisters["SP"].value - 2)));
            GeneralPurposeRegisters["SP"].value -= 2;
        }


        //[bx/bp+di/si]
        public ushort calculateAddress()
        {
            ushort result = 0;

            if (BX_Memory.Checked) result += GeneralPurposeRegisters["BX"].value;
            if (BP_Memory.Checked) result += GeneralPurposeRegisters["BP"].value;
            if (DI_Memory.Checked) result += GeneralPurposeRegisters["DI"].value;
            if (SI_Memory.Checked) result += GeneralPurposeRegisters["SI"].value;
            if (DISP_Memory.Checked) result += GeneralPurposeRegisters["DISP"].value;

            return result;
        }


        public bool validateInput(string input)
        {
            return (Regex.IsMatch(input, @"^[A-F0-9]+$") && input.Length <= 4);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void updateDisplay()
        {
            AX.Text = GeneralPurposeRegisters["AX"].value.ToString("X4");
            BX.Text = GeneralPurposeRegisters["BX"].value.ToString("X4");
            CX.Text = GeneralPurposeRegisters["CX"].value.ToString("X4");
            DX.Text = GeneralPurposeRegisters["DX"].value.ToString("X4");

            BP.Text = GeneralPurposeRegisters["BP"].value.ToString("X4");
            DI.Text = GeneralPurposeRegisters["DI"].value.ToString("X4");
            SI.Text = GeneralPurposeRegisters["SI"].value.ToString("X4");
            SP.Text = GeneralPurposeRegisters["SP"].value.ToString("X4");
            DISP.Text = GeneralPurposeRegisters["DISP"].value.ToString("X4");
        }

        private void SetValues_Click(object sender, EventArgs e)
        {
            if (validateInput(AX_Value.Text)) GeneralPurposeRegisters["AX"].value = Convert.ToUInt16(AX_Value.Text, 16);
            if (validateInput(BX_Value.Text)) GeneralPurposeRegisters["BX"].value = Convert.ToUInt16(BX_Value.Text, 16);
            if (validateInput(CX_Value.Text)) GeneralPurposeRegisters["CX"].value = Convert.ToUInt16(CX_Value.Text, 16);
            if (validateInput(DX_Value.Text)) GeneralPurposeRegisters["DX"].value = Convert.ToUInt16(DX_Value.Text, 16);

            if (validateInput(BP_Value.Text)) GeneralPurposeRegisters["BP"].value = Convert.ToUInt16(BP_Value.Text, 16);
            if (validateInput(DI_Value.Text)) GeneralPurposeRegisters["DI"].value = Convert.ToUInt16(DI_Value.Text, 16);
            if (validateInput(SI_Value.Text)) GeneralPurposeRegisters["SI"].value = Convert.ToUInt16(SI_Value.Text, 16);
            if (validateInput(SP_Value.Text)) GeneralPurposeRegisters["SP"].value = Convert.ToUInt16(SP_Value.Text, 16);
            if (validateInput(DISP_Value.Text)) GeneralPurposeRegisters["DISP"].value = Convert.ToUInt16(DISP_Value.Text, 16);

            updateDisplay();
        }

        bool BPCheckedBefore = false;
        private void BP_Memory_CheckedChanged(object sender, EventArgs e)
        {
            BPCheckedBefore = true;
        }

        bool BXCheckedBefore = false;
        private void BX_Memory_CheckedChanged(object sender, EventArgs e)
        {
            BXCheckedBefore = true;
        }

        bool DICheckedBefore = false;
        private void DI_Memory_CheckedChanged(object sender, EventArgs e)
        {
            DICheckedBefore = true;
        }

        bool SICheckedBefore = false;
        private void SI_Memory_CheckedChanged(object sender, EventArgs e)
        {
            SICheckedBefore = true;
        }

        bool DISPCheckedBefore = false;
        private void DISP_Memory_CheckedChanged(object sender, EventArgs e)
        {
            DISPCheckedBefore = true;
        }


        private void BP_Memory_Click(object sender, EventArgs e)
        {
            RadioButton button = ((RadioButton)sender);
            if (button.Checked && !BPCheckedBefore)
                button.Checked = false;
            else
            {
                button.Checked = true;
                BPCheckedBefore = false;
            }
        }

        private void BX_Memory_Click(object sender, EventArgs e)
        {
            RadioButton button = ((RadioButton)sender);
            if (button.Checked && !BXCheckedBefore)
                button.Checked = false;
            else
            {
                button.Checked = true;
                BXCheckedBefore = false;
            }
        }

        private void DI_Memory_Click(object sender, EventArgs e)
        {
            RadioButton button = ((RadioButton)sender);
            if (button.Checked && !DICheckedBefore)
                button.Checked = false;
            else
            {
                button.Checked = true;
                DICheckedBefore = false;
            }
        }

        private void SI_Memory_Click(object sender, EventArgs e)
        {
            RadioButton button = ((RadioButton)sender);
            if (button.Checked && !SICheckedBefore)
                button.Checked = false;
            else
            {
                button.Checked = true;
                SICheckedBefore = false;
            }
        }

        private void DISP_Memory_Click(object sender, EventArgs e)
        {
            RadioButton button = ((RadioButton)sender);
            if (button.Checked && !DISPCheckedBefore)
                button.Checked = false;
            else
            {
                button.Checked = true;
                DISPCheckedBefore = false;
            }
        }

        private void toSrc_Click(object sender, EventArgs e)
        {
            srcSelector.Text = calculateAddress().ToString("X4");
        }

        private void toDest_Click(object sender, EventArgs e)
        {
            destSelector.Text = calculateAddress().ToString("X4");
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(instruction))
            {
                object[] parameters = new object[] { srcSelector.Text, destSelector.Text };
                int index = 0;

                foreach (object obj in parameters)
                {
                    string selection = (string)obj;
                    Register16 reg16 = null;

                    try
                    {
                        reg16 = GeneralPurposeRegisters[selection];
                    }
                    catch (Exception) { };

                    if (reg16 is not null) parameters[index] = reg16;
                    else if (!string.IsNullOrEmpty(selection)) parameters[index] = Convert.ToUInt16(selection, 16);
                    index++;
                }

                switch (instruction.ToLower())
                {

                    case "mov":
                        if (parameters[0] is Register16 && parameters[1] is Register16) MOV((Register16)parameters[0], (Register16)parameters[1]);
                        else if (parameters[0] is Register16 && parameters[1] is ushort) MOV((Register16)parameters[0], (ushort)parameters[1]);
                        else if (parameters[0] is ushort && parameters[1] is Register16) MOV((ushort)parameters[0], (Register16)parameters[1]);
                        break;

                    case "xchg":
                        if (parameters[0] is Register16 && parameters[1] is Register16) XCHG((Register16)parameters[0], (Register16)parameters[1]);
                        else if (parameters[0] is Register16 && parameters[1] is ushort) XCHG((Register16)parameters[0], (ushort)parameters[1]);
                        else if (parameters[0] is ushort && parameters[1] is Register16) XCHG((ushort)parameters[0], (Register16)parameters[1]);
                        break;

                    case "push":
                        if (parameters[0] is Register16) PUSH((Register16)parameters[0]);
                        if (parameters[0] is ushort) PUSH((ushort)parameters[0]);
                        break;
                    case "pop":
                        if (parameters[1] is Register16) POP((Register16)parameters[1]);
                        if (parameters[1] is ushort) POP((ushort)parameters[1]);
                        break;

                }
                updateDisplay();
            }
        }

        private void MOV_Radio_CheckedChanged(object sender, EventArgs e)
        {
            instruction = "MOV";
        }

        private void XCHG_Radio_CheckedChanged(object sender, EventArgs e)
        {
            instruction = "XCHG";
        }

        private void PUSH_Radio_CheckedChanged(object sender, EventArgs e)
        {
            instruction = "PUSH";
        }

        private void POP_Radio_CheckedChanged(object sender, EventArgs e)
        {
            instruction = "POP";
        }
    }
}
