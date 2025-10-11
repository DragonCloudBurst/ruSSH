using Spectre.Console;

public class userInterface
{

    public userInterface()
    {
        Layout layout = new Layout("Root")
        .SplitColumns(
        new Layout("Left"),
        new Layout("Right")
            .SplitRows(
                new Layout("Top"),
                new Layout("Bottom")));

        layout["Left"].Ratio(3);
        layout["Right"].Ratio(1);
        layout["Top"].Ratio(1);
        layout["Bottom"].Ratio(2);

        var gamePanel = new Panel(
                Align.Center(
                    // add player and level generation logic here
                    new Markup("placeholder"),
                    VerticalAlignment.Middle));
            
        var panelStats = new Panel(
                Align.Center(
                    // add in the actual variables here once we have implemented them!
                    // i would like to use the bar widget eventually somehow if possible.
                    new Markup("HP [   ]\r\n" +
                    "ATK [   ]\r\n" +
                    "DEF [   ]\r\n"),

                    VerticalAlignment.Middle));

        var panelFlavorText = new Panel(
                Align.Center(
                    new Markup(""),
                    VerticalAlignment.Middle));

        gamePanel.Border = BoxBorder.Ascii;
        panelStats.Border = BoxBorder.Ascii;
        panelFlavorText.Border = BoxBorder.Ascii;

        gamePanel.Padding = new Padding(5, 5, 5, 5);
        panelStats.Padding = new Padding(2, 2, 2, 2);
        panelFlavorText.Padding = new Padding(1, 1, 1, 1);


        layout["Left"].Update(gamePanel);
        layout["Top"].Update(panelStats);
        layout["Bottom"].Update(panelFlavorText);

        AnsiConsole.Write(layout);
    }

}