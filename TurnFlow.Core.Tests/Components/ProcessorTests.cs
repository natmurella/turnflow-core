using System.Security.Cryptography.X509Certificates;
using TurnFlow.Core.Components.Processors;
using TurnFlow.Core.Components;

namespace TurnFlow.Core.Tests.Components;

public class AdditionProcessor : BaseProcessor
{
    public AdditionProcessor(
        Dictionary<string, IComponent> components
    ) : base(components)
    {
        
    }

    public override int Recalculate()
    {   
        int total = 0;
        foreach (var component in dependentComponents.Values)
        {
            total += component.Read();
        }

        return total;
    }
}

public class ProcessorTests
{
    [Fact]
    public void Addition()
    {
        Component a = new Component();
        Component b = new Component();
        Component c = new Component();

        a.Add(5);
        b.Add(10);
        c.Add(15);

        Dictionary<string, IComponent> components = new Dictionary<string, IComponent>()
        {
            {"a", a},
            {"b", b},
            {"c", c}
        };

        AdditionProcessor processor = new AdditionProcessor(components);
        int value = processor.Recalculate();

        Assert.Equal(30, value);
    }
}