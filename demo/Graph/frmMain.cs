﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LuaSharpVM.Core;
using LuaSharpVM.Decompiler;
using LuaSharpVM.Disassembler;


namespace Graph
{
    public partial class frmMain : Form
    {
        private frmGraph activeGraph;
        public frmMain()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select Binary File";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(fileDialog.FileName))
                    return;

                var luaF = new LuaCFile(File.ReadAllBytes(fileDialog.FileName));
                LuaDecoder d = new LuaDecoder(luaF);
                LuaWriter writer = new LuaWriter(ref d);
                activeGraph = new frmGraph(writer);
                activeGraph.Show();

                lstFuncs.Items.Clear();
                foreach (var f in activeGraph.Writer.LuaFunctions)
                    lstFuncs.Items.Add(f.ToString());

                txtLuaCode.Text = activeGraph.Writer.LuaScript;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (activeGraph == null)
                return;

            if (activeGraph.Writer.LuaFunctions.Count < lstFuncs.SelectedIndex)
                return;
            activeGraph.TargetFunc = lstFuncs.SelectedIndex;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
