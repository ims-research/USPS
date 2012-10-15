namespace USPS.Code
{
    public class TypeNode
    {
        public string Name { get; set; }
        public string GlobalGUID { get; set; }
        public string InstanceGUID { get; set; }
        public ServiceBlock.Types BlockType { get; set; }

        public TypeNode(ServiceBlock.Types type)
        {
            BlockType = type;
        }

    }
}