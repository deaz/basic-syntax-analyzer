using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SyntaxAnalyzer;

namespace SyntaxAnalyzer.Interface
{
    public partial class MainForm : Form
    {
        private const string ApplicationName = "Синтаксический анализатор";
        private const string DefaultFileName = "Текстовый документ";

        string fileName;
        bool isSaved;
        bool isNewFile;

        public MainForm()
        {
            InitializeComponent();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            fileName = DefaultFileName;
            setText();
            isSaved = true;
            isNewFile = true;
        }

        private void setText()
        {
            this.Text = fileName + " - " + ApplicationName;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                closeToolStripMenuItem_Click(sender, e);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    mainRichTextBox.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                    fileName = openFileDialog.FileName;
                    isSaved = true;
                    isNewFile = false;
                    setText();
                }
                else
                {
                    mainRichTextBox.LoadFile(fileName, RichTextBoxStreamType.PlainText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    mainRichTextBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                    fileName = saveFileDialog.FileName;
                    isSaved = true;
                    setText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (isNewFile)
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    mainRichTextBox.SaveFile(fileName, RichTextBoxStreamType.PlainText);
                }
                isNewFile = false;
                isSaved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Redo();
        }

        private void mainRichTextBox_TextChanged(object sender, EventArgs e)
        {
            isSaved = false;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeFile();
        }

        private DialogResult closeFile()
        {
            DialogResult dialogResult = DialogResult.Yes;

            if (!isSaved)
            {
                dialogResult = MessageBox.Show("Сохранить \"" + fileName + "\"?", "Сохранение", MessageBoxButtons.YesNoCancel);

                if (dialogResult == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem_Click(this, EventArgs.Empty);
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    return dialogResult;
                }
            }

            mainRichTextBox.Clear();
            fileName = DefaultFileName;
            isNewFile = true;
            isSaved = true;
            setText();

            return dialogResult;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeToolStripMenuItem_Click(sender, e);
            fileName = DefaultFileName;
            isNewFile = true;
            isSaved = true;
            setText();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (closeFile() != DialogResult.Cancel)
            {
                Application.Exit();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Paste();
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Cut();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Redo();
        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Undo();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Cut();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Paste();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Copy();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeFile() == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            mainRichTextBox.Clear();
        }

        private void visiblityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !splitContainer.Panel2Collapsed;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> messages = new List<string>();

            Parser parser = new Parser(mainRichTextBox.Text, messages);
            parser.Start();

            outputListBox.Items.Clear();
            foreach (string message in messages)
            {
                outputListBox.Items.Add(message);
            }
        }

        private void openDocument(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void problemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Problem.docx");
        }

        private void grammarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Grammar.docx");

        }

        private void classificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Hierarchy.docx");
        }

        private void algorithmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Algorithm.docx");
        }

        private void diagnosticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Diagnostics.docx");
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Test.docx");
        }

        private void literatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Literature.docx");
        }

        private void listingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDocument("Listing.docx");
        }

        private void startToolStripButton_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem_Click(sender, e);
        }
    }
}
