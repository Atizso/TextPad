using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Speech.Synthesis;

namespace TextPad
{
    public partial class TextPad : Form
    {
        private string CurrentFile = "";
        private bool Saved = true;
        private int line = 0;
        private int column = 0;
        List<string> listFiles = new List<string>();

        public TextPad()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                StreamWriter sw = new StreamWriter(this.Text = Path.GetFileName(CurrentFile) + " - TextPad+");
                sw.Write(richTextBox1.Text);
                sw.Close();
                Saved = true;
                if (CurrentFile == "")
                {
                    CurrentFile = "Untitled";
                }
            }
            catch (Exception)
            {
                openToolStripMenuItem_Click(sender, e);
            }
          
            this.Text = Path.GetFileName(CurrentFile) + " - TextPad+";
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog svaf = new SaveFileDialog();
            svaf.Title = "Save File";
            svaf.Filter = "Plain Text|*.txt|Rich Text Documentum|*.rtf|Open Documentum Text|*.odt";
            svaf.CheckFileExists = false;
            svaf.CheckPathExists = false;
            svaf.RestoreDirectory = true;
            svaf.DefaultExt = "txt";
            svaf.AddExtension = true;
            svaf.FileName = "Untitled";
            if (svaf.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(svaf.FileName);
                sw.Write(richTextBox1.Text);
                sw.Close();
                Saved = true;
                CurrentFile = svaf.FileName;
                this.Text = Path.GetFileName(CurrentFile) + " - TextPad+";
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Saved == false)
            {
                DialogResult dr = MessageBox.Show("File not saved do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.Yes) { richTextBox1.Clear();
                    this.Text = "New Document - TextPad+";
                }
            }else { richTextBox1.Clear();
                this.Text = "New Document - TextPad+";
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Plain Text|*.txt|Rich Text Documentum|*.rtf|Open Documentum Text|*.odt|All Text File|*.txt; *.rtf; *.odt";
            ofd.DefaultExt = "txt";
            ofd.Title = "Open a File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                richTextBox1.Text = sr.ReadToEnd();
                sr.Close();
                CurrentFile = ofd.FileName;
                this.Text = Path.GetFileName(CurrentFile) + " - TextPad+";
                Saved = true;
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, new System.Drawing.Font("Arial", 16, FontStyle.Regular), Brushes.Black, 100, 100);
        }

        private void timeDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void textFontToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.ShowColor = true;
            fd.ShowEffects = true;
            if (fd.ShowDialog() == DialogResult.OK & !String.IsNullOrEmpty(richTextBox1.Text))
            {
                richTextBox1.SelectionFont = fd.Font;
                richTextBox1.SelectionColor = fd.Color;
            }
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize += 1.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize -= 1.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize = 8.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void foregroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            richTextBox1.ForeColor = cd.Color;
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            richTextBox1.BackColor = cd.Color;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a free text editor. \r\n Version 0.1a", "About TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void fontToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.ShowColor = true;
            fd.ShowEffects = true;
            if (fd.ShowDialog() == DialogResult.OK & !String.IsNullOrEmpty(richTextBox1.Text))
            {
                richTextBox1.SelectionFont = fd.Font;
                richTextBox1.SelectionColor = fd.Color;
            }
        }

        private void foregroundToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            richTextBox1.ForeColor = cd.Color;
        }

        private void backgroundToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            richTextBox1.BackColor = cd.Color;
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.WordWrap = true;
        }

        private void zoomInToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize += 1.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void zoomOutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize -= 1.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void resetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize = 8.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void jumpToBottomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void zoomInToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize += 1.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void zoomOutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize -= 1.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void resetToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            float currentSize;
            currentSize = richTextBox1.Font.Size;
            currentSize = 8.0F;
            richTextBox1.Font = new System.Drawing.Font(richTextBox1.Font.Name, currentSize,
            richTextBox1.Font.Style, richTextBox1.Font.Unit);
        }

        private void jumpToTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.ScrollToCaret();
        }

        private void jumpToBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.ScrollToCaret();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            line = 1 + richTextBox1.GetLineFromCharIndex(richTextBox1.GetFirstCharIndexOfCurrentLine());
            column = 1 + richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexOfCurrentLine();
            toolStripStatusLabel1.Text = "line: " + line.ToString() + " | column: " + column.ToString() + ";";
        }

        private void TextPad_Load(object sender, EventArgs e)
        {
            notifyIcon1.Text = "TextPad+";
            this.Text = "Untitled - TextPad+";
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = true;
            WindowState = FormWindowState.Normal;
        }

        private void TextPad_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            } else if (FormWindowState.Normal == this.WindowState){
                notifyIcon1.Visible = true;
            }
        }

        private void saveWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog svaf = new SaveFileDialog();

            svaf.Filter = "Plain Text|*.txt|Rich Text Documentum|*.rtf|Open Documentum Text|*.odt";
            svaf.Title = "Save File";
            svaf.DefaultExt = "txt";
            svaf.CheckFileExists = false;
            svaf.CheckPathExists = false;
            svaf.RestoreDirectory = true;
            svaf.AddExtension = true;
            svaf.FileName = "Untitled";
            if (svaf.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(svaf.FileName);
                sw.Write(richTextBox1.Text);
                sw.Close();
                Saved = true;
                CurrentFile = svaf.FileName;
                this.Text = Path.GetFileName(CurrentFile) + " - TextPad+";
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a free text editor.", "About TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void jumpToTopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.ScrollToCaret();
        }

        private void toolStripMenuItem34_Click(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
            toolStripStatusLabel2.Text = "Read Mode";
        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = false;
            toolStripStatusLabel2.Text = "Write Mode";
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            richTextBox1.WordWrap = true;
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            richTextBox1.WordWrap = false;
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void sendFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/forms/d/e/1FAIpQLSe7wSHy0Iv4v0isVZn5N-IGDN6qzX8e3vAdIoI72McJ0pCGmQ/viewform");
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/forms/d/e/1FAIpQLSe7wSHy0Iv4v0isVZn5N-IGDN6qzX8e3vAdIoI72McJ0pCGmQ/viewform");
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = true;
            WindowState = FormWindowState.Normal;
        }

        private void sendFeedbackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/forms/d/e/1FAIpQLSe7wSHy0Iv4v0isVZn5N-IGDN6qzX8e3vAdIoI72McJ0pCGmQ/viewform");
        }

        private void webPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sites.google.com/view/textpad-free-download/home");
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sites.google.com/view/textpad-free-download/home");
        }

        private void webPageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sites.google.com/view/textpad-free-download/home");
        }

        private void onToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Visible = true;
            toolStripStatusLabel2.Visible = true;
        }

        private void offToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Visible = false;
            toolStripStatusLabel2.Visible = false;
        }

        private void onToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
            toolStripStatusLabel2.Text = "Read Mode;";
        }

        private void offToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = false;
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.WordWrap = false;
            toolStripStatusLabel2.Text = "Write Mode;";
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextPad newTextPad = new TextPad();
            newTextPad.Show();
        }

        private void exportToPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog EtPDF = new SaveFileDialog() { Filter="PDF file|*.pdf", ValidateNames = true, FileName = "Untitled"})
            {
                if (EtPDF.ShowDialog() == DialogResult.OK)
                {
                    if (richTextBox1.Text == "")
                    {
                        MessageBox.Show("Write something!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } else { 

                        iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4.Rotate());
                        try
                        {
                            PdfWriter.GetInstance(doc, new FileStream(EtPDF.FileName, FileMode.Create));
                            doc.Open();
                            doc.Add(new iTextSharp.text.Paragraph(richTextBox1.Text));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            doc.Close();
                            Saved = true;
                            CurrentFile = EtPDF.FileName;
                            this.Text = Path.GetFileName(CurrentFile) + " - TextPad+";
                        }
                    }
                }
            }
        }

        private void TextPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Saved == false)
            {
                DialogResult dr = MessageBox.Show("Changes not saved!\r\nDo you want to save it?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.Yes)
                {
                    if (CurrentFile == "") saveAsToolStripMenuItem_Click(sender, e);
                    else richTextBox1.SaveFile(CurrentFile, RichTextBoxStreamType.RichText);
                    this.Text = Path.GetFileName(CurrentFile) + " - TextPad+";
                }
                else if (dr == DialogResult.No)
                {
                    Application.Exit();
                }
                else if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }       
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
                Saved = true;
            else
                Saved = false;
        }

        private void maleVoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer voice = new SpeechSynthesizer();
            voice.SelectVoiceByHints(VoiceGender.Male);
            voice.SpeakAsync(richTextBox1.Text);
        }

        private void femaleVoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer voice = new SpeechSynthesizer();
            voice.SelectVoiceByHints(VoiceGender.Female);
            voice.SpeakAsync(richTextBox1.Text);
        }
    }
}
