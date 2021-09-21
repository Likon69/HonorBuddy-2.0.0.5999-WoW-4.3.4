using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx;
using Styx.Logic.Inventory.Frames.Merchant;

namespace ProfileHelper
{
    public partial class ToolForm : Form
    {
        public ToolForm()
        {
            InitializeComponent();
        }

        private void ToolForm_Load(object sender, EventArgs e)
        {
            UpdateStuff();
        }

        public void UpdateStuff()
        {
            dgwGameObjects.DataSource = ObjectManager.GetObjectsOfType<WoWGameObject>(false, false).
    Where(o => o.BaseAddress != 0 && o.IsValid).OrderBy(o => o.Distance).ToList();

            dgwInventoryItems.DataSource = StyxWoW.Me.CarriedItems.OrderBy(o => o.Name).ToList();

            dgwMyAuras.DataSource = StyxWoW.Me.Auras.Values.OrderBy(a => a.Name).ToList();

            dgwNpcs.DataSource = ObjectManager.GetObjectsOfType<WoWUnit>(false, false).
                Where(u => u.BaseAddress != 0 && u.IsValid).OrderBy(u => u.Distance).ToList();

            if (StyxWoW.Me.CurrentTarget != null)
                dgwTargetAuras.DataSource = StyxWoW.Me.CurrentTarget.Auras.Values.OrderBy(a => a.Name).ToList();

            if (MerchantFrame.Instance.IsVisible)
                dgwMerchantItems.DataSource = MerchantFrame.Instance.GetAllMerchantItems().ToList();
        }

        private void dgwGameObjects_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dgwNpcs_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dgwMyAuras_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dgwTargetAuras_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dgwInventoryItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void mainTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStuff();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateStuff();
        }
    }
}
