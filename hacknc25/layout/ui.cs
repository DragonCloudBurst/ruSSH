using Spectre.Console;

public class userInterface
{
    public Layout layout = new Layout("Root")
    .SplitColumns(
        new Layout("Left"),
        new Layout("Right")
            .SplitRows(
                new Layout("Top"),
                new Layout("Bottom")));
    
    public void Write()
    {
        // Update the left column
        layout["Left"].Update(
            new Panel(
                Align.Center(
                    // add player and level generation logic here
                    new Markup("placeholder"),
                    VerticalAlignment.Middle))
                .Expand());

        

        // Render the layout
        AnsiConsole.Write(layout);
    }
}