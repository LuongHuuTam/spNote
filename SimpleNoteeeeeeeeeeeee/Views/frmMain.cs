using SimpleNoteeeeeeeeeeeee.Controllers;
using SimpleNoteeeeeeeeeeeee.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleNoteeeeeeeeeeeee.Views
{
    public partial class frmMain : Form
    {
        private bool flag;
        public frmMain()
        {
            InitializeComponent();
            flag = true;
            loadNote();
        }
        private void loadNote()
        {
            listViewNote.Items.Clear();
            List<Note> ln = NoteControllers.getListNote();
            for (int i = ln.Count - 1; i >= 0; i--)
            {
                if (ln[i].IsPin == true)
                {
                    ListViewItem n = new ListViewItem("$");
                    n.SubItems.Add(ln[i].Title);
                    listViewNote.Items.Add(n);
                }
            }

            for (int i=ln.Count-1;i>=0;i--)
            {
                if (ln[i].IsPin==false)
                {
                    ListViewItem n = new ListViewItem("*");
                    n.SubItems.Add(ln[i].Title);
                    listViewNote.Items.Add(n);
                }
            }
            clearControl();
            btnDelete.Enabled = btnInfo.Enabled = btnSave.Enabled = checkPin.Enabled = false;
        }
        private void loadTrash()
        {
            listViewNote.Items.Clear();
            List<Note> ln = TrashControllers.getListTrash();
            for (int i = ln.Count - 1; i >= 0; i--)
            {
                ListViewItem n = new ListViewItem("**");
                n.SubItems.Add(ln[i].Title);
                listViewNote.Items.Add(n);
            }
            clearControl();
            btnDelete.Enabled = btnInfo.Enabled = btnSave.Enabled = checkPin.Enabled = false;
        }
        private void clearControl()
        {
            tbTag.Text = tbTitle.Text = rtbNote.Text = "";
            checkPin.Checked = false;
        }
        private void btnMode_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = btnInfo.Enabled = btnSave.Enabled = false;
            if (flag)
            {
                flag = !flag;
                this.btnMode.Text = Text = "Trash";
                this.btnSave.Visible = false;
                this.btnDelete.Text = "Delete Forever";
                this.btnInfo.Text = "Restore Note";
                this.btnNew.Enabled = false;
                this.checkPin.Visible = false;
                loadTrash();
            }
            else
            {
                flag = !flag;
                this.btnMode.Text = Text = "All Note";
                this.btnSave.Visible = true;
                this.btnDelete.Text = "Delete";
                this.btnInfo.Text = "Info";
                this.btnNew.Enabled = true;
                this.checkPin.Visible = true;
                loadNote();
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            Note ln = NoteControllers.getNote("New Note");
            if (ln != null)
            {
                MessageBox.Show("New note has been created.\nUse before creating a new one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Note note = new Note();
            note.ID = NoteControllers.getID();
            note.Title = "New Note";
            note.Modified = DateTime.Now;
            note.IsTrash = false;
            note.IsPin = false; 
            if (NoteControllers.addNote(note) == false)
                return;
            loadNote();
        }
        private void listViewNote_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = btnInfo.Enabled = btnSave.Enabled = checkPin.Enabled = true;
            if (flag)
            {
                Note note = NoteControllers.getNote(listViewNote.SelectedItems[0].SubItems[1].Text);
                tbTitle.Text = note.Title;
                rtbNote.Text = note.Descriptions;
                checkPin.Checked = note.IsPin.Value;
                string str = "";
                foreach (Tag i in note.Tags)
                {
                    str += i.ToString() + " ";
                }
                tbTag.Text = str;
            }
            else
            {
                Note note = TrashControllers.getTrash(listViewNote.SelectedItems[0].SubItems[1].Text);
                tbTitle.Text = note.Title;
                rtbNote.Text = note.Descriptions;
                string str = "";
                foreach (Tag i in note.Tags)
                {
                    str += i.ToString() + " ";
                }
                tbTag.Text = str;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {            
            Note note = NoteControllers.getNote(listViewNote.SelectedItems[0].SubItems[1].Text);
            note.Title = tbTitle.Text;
            note.Descriptions = rtbNote.Text;
            note.Modified = DateTime.Now;
            if (checkPin.Checked == true)
                note.IsPin = true;
            else
                note.IsPin = false;
            note.IsTrash = false;
            string[] str = tbTag.Text.Split(' ');
            foreach (string i in str)
            {
                if (i != "" && i != " ")
                {
                    Tag tag = new Tag() { Tags = i };
                    TagControllers.addTag(tag);
                    note.Tags.Add(tag);
                }
            }
            NoteControllers.updateNote(note);
            loadNote();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {            
            if (flag)
            {
                Note note = NoteControllers.getNote(listViewNote.SelectedItems[0].SubItems[1].Text);
                if (note.Title == "New Note" && note.Descriptions.Length == 0)
                {
                    NoteControllers.deleteNote(note);
                    loadNote();
                }
                else
                {
                    note.IsTrash = true;
                    NoteControllers.updateNote(note);
                    loadNote();
                }
            }
            else
            {
                Note trash = TrashControllers.getTrash(listViewNote.SelectedItems[0].SubItems[1].Text);
                NoteControllers.deleteNote(trash);
                loadTrash();
            }
        }
        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                Note note = NoteControllers.getNote(listViewNote.SelectedItems[0].SubItems[1].Text);
                string[] arr = note.Descriptions.Split(' ');
                string[] arr1 = note.Descriptions.Split('\n');
                int a = arr.Count() + arr1.Count() - 1;
                MessageBox.Show(note.Descriptions.Length + "character\n" + a + "words\n" + "Modified: " + note.Modified.Value.ToString() + "\nPin to top: " + note.IsPin.Value, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Note note = TrashControllers.getTrash(listViewNote.SelectedItems[0].SubItems[1].Text);
                note.IsTrash = false;
                NoteControllers.updateNote(note);
                loadTrash();
            }
        }
        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (flag)
            {
                if (radioTitle.Checked==true)
                {
                    if (tbSearch.Text.Length == 0)
                        loadNote();
                    else
                    {
                        listViewNote.Items.Clear();
                        List<Note> ln = NoteControllers.getListNote(tbSearch.Text);
                        foreach (Note n in ln)
                        {
                            ListViewItem listViewItem = new ListViewItem("*");
                            listViewItem.SubItems.Add(n.Title);
                            listViewNote.Items.Add(listViewItem);
                        }
                    }
                }
                else
                {
                    if (tbSearch.Text.Length == 0)
                        loadNote();
                    else
                    {
                        listViewNote.Items.Clear();
                        List<Note> ln = TagControllers.getListNote(tbSearch.Text);
                        foreach (Note n in ln)
                        {
                            ListViewItem listViewItem = new ListViewItem("*");
                            listViewItem.SubItems.Add(n.Title);
                            listViewNote.Items.Add(listViewItem);
                        }
                    }
                }
            }
            else
            {
                if (radioTitle.Checked == true)
                {
                    if (tbSearch.Text.Length == 0)
                        loadTrash();
                    else
                    {
                        listViewNote.Items.Clear();
                        List<Note> ln = TrashControllers.getListTrash(tbSearch.Text);
                        foreach (Note n in ln)
                        {
                            ListViewItem listViewItem = new ListViewItem("*");
                            listViewItem.SubItems.Add(n.Title);
                            listViewNote.Items.Add(listViewItem);
                        }
                    }
                }
                else
                {
                    if (tbSearch.Text.Length == 0)
                        loadTrash();
                    else
                    {
                        listViewNote.Items.Clear();
                        List<Note> ln = TagControllers.getListTrash(tbSearch.Text);
                        foreach (Note n in ln)
                        {
                            ListViewItem listViewItem = new ListViewItem("*");
                            listViewItem.SubItems.Add(n.Title);
                            listViewNote.Items.Add(listViewItem);
                        }
                    }
                }
            }
        }
        private void tagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTag frmTag = new frmTag();
            frmTag.Show();
        }
        private void newNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(flag)
            {
                Note ln = NoteControllers.getNote("New Note");
                if (ln != null)
                {
                    MessageBox.Show("New note has been created.\nUse before creating a new one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Note note = new Note();
                note.ID = NoteControllers.getID();
                note.Title = "New Note";
                note.Modified = DateTime.Now;
                note.IsTrash = false;
                note.IsPin = false;
                if (NoteControllers.addNote(note) == false)
                    return;
                loadNote();
            }
            else
            {
                return;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}