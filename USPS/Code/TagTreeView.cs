using System.Web.UI.WebControls;


namespace USPS.Code
{
public class TagTreeView : TreeView
    {
        protected override TreeNode CreateNode()
        {
            return new TagTreeNode(this, false);
        }
    }

    public class TagTreeNode : TreeNode
    {
        public object Tag { get; set; }
        public TagTreeNode() : base()
        {
        }

        public TagTreeNode(TreeView owner, bool isRoot)
            : base(owner, isRoot)
        {
        }

        protected override void LoadViewState(object state)
        {
            object[] arrState = state as object[];

            Tag = arrState[0];
            base.LoadViewState(arrState[1]);
        }
        
        protected override object SaveViewState()
        {
            object[] arrState = new object[2];
            arrState[1] = base.SaveViewState();
            arrState[0] = Tag;
            return arrState;
        }
    }

}