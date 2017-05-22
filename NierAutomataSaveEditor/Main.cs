using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NierAutomataSaveEditor.API;
using NierAutomataSaveEditor.API.Objects;

namespace NierAutomataSaveEditor
{
    public partial class Main : Form
    {
        #region Properties
        public SaveFile Save { get; private set; }
        #endregion

        public Main()
        {
            InitializeComponent();
        }

        #region Form Functions
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            if (!Parser.CheckValid(openFileDialog1.FileName))
            {
                MessageBox.Show("File is eiter corrupted or not correct!", "Nier Automata Save Editor");
                return;
            }

            listItems.Items.Clear();

            Save = Parser.Parse(openFileDialog1.FileName);

            txtName.Text = Save.Name;
            numMoney.Value = Save.Money;
            numEXP.Value = Save.EXP;
            foreach(KeyValuePair<short, short> item in Save.Items)
            {
                ListViewItem lvi = new ListViewItem(item.Key.ToString());

                lvi.SubItems.Add(item.Value.ToString());

                listItems.Items.Add(lvi);
            }
            btnSave.Enabled = true;
        }

        private void listItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listItems.SelectedItems.Count == 0)
                return;

            numID.Value = int.Parse(listItems.SelectedItems[0].SubItems[0].Text);
            numAmount.Value = int.Parse(listItems.SelectedItems[0].SubItems[1].Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listItems.Items.ContainsKey(numID.Value.ToString()))
            {
                MessageBox.Show("This item already exists in the item list!", "Nier Automata Save Editor");
                return;
            }

            ListViewItem lvi = new ListViewItem(numID.Value.ToString());

            lvi.SubItems.Add(numAmount.Value.ToString());

            listItems.Items.Add(lvi);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItems.Count == 0)
                return;

            listItems.Items.Remove(listItems.SelectedItems[0]);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItems.Count == 0)
                return;

            listItems.SelectedItems[0].SubItems[0].Text = numID.Value.ToString();
            listItems.SelectedItems[0].SubItems[1].Text = numAmount.Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            File.Copy(openFileDialog1.FileName, openFileDialog1.FileName + ".backup", true);

            Save.Money = (int)numMoney.Value;
            Save.EXP = (int)numEXP.Value;
            Save.Items.Clear();
            foreach(ListViewItem item in listItems.Items)
                Save.Items.Add(short.Parse(item.SubItems[0].Text), short.Parse(item.SubItems[1].Text));

            if (Save.Save())
                MessageBox.Show("File saved!", "Nier Automata Save Editor");
            else
                MessageBox.Show("Failed to save file!", "Nier Automata Save Editor");
        }
        #endregion
    }
}
