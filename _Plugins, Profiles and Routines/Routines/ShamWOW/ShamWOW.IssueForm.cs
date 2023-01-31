/*
 * NOTE:    DO NOT POST ANY MODIFIED VERSIONS OF THIS TO THE FORUMS.
 * 
 *          DO NOT UTILIZE ANY PORTION OF THIS COMBAT CLASS WITHOUT
 *          THE PRIOR PERMISSION OF AUTHOR.  PERMITTED USE MUST BE
 *          ACCOMPANIED BY CREDIT/ACKNOWLEDGEMENT TO ORIGINAL AUTHOR.
 * 
 * ShamWOW Shaman CC
 * 
 * Author:  Bobby53
 * 
 * See the ShamWOW.chm file for Help
 *
 */
using System;
using System.Windows.Forms;

namespace Bobby53
{
    public partial class IssueForm : Form
    {
        public IssueForm()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(
            this,
            "http://www.buddyforum.de/newreply.php?p=120595&noquote=1",
            HelpNavigator.TableOfContents,
            "Contents"
            );
        }
    }
}
