using SimpleNoteeeeeeeeeeeee.Controllers;
using SimpleNoteeeeeeeeeeeee.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleNoteeeeeeeeeeeee.Views
{
    public partial class frmTag : Form
    {
        public frmTag()
        {
            InitializeComponent();
            loadTag();
        }
        private void loadTag()
        {
            listView1.Items.Clear();
            List<Tag> tags = TagControllers.getListTag();
            foreach(var t in tags)
            {
                ListViewItem listViewItem = new ListViewItem(t.ToString());
                listView1.Items.Add(listViewItem);
            }
            button1.Enabled = false;
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tag tag = TagControllers.getTag(listView1.SelectedItems[0].Text);
            TagControllers.deleteTag(tag);
            loadTag();
        }
    }
}
