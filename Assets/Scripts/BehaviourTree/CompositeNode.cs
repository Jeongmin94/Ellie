using System.Collections.Generic;

public abstract class CompositeNode : Node
{
    public List<Node> children = new List<Node>();

    public override Node Clone()
    {
        CompositeNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());    // 자식들을 모두 클론시키고 children 적용
        return node;
    }
}
