using System.Web.UI.WebControls;

namespace USPS.Code
{
    public class TypeNode : Node
    {
        public ServiceBlock.Types BlockType { get; set; }

        public TypeNode(ServiceBlock.Types type)
        {
            BlockType = type;
        }

    }
}