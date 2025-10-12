using Spectre.Console;

public class GameUI {
    private readonly Layout _layout;

    public Layout Layout => _layout;
    private int width;

    public GameUI(int Width) {
        width = Width;
        _layout = new Layout("Root").SplitRows(
            new Layout("GameArea") { Size = Console.WindowHeight - 11 },
            new Layout("Messages") { Size = 5 },
            new Layout("StatBlock") { Size = 3}
        );

        _layout["GameArea"].Update(new Panel("").Header("Game"));
        _layout["Messages"].Update(new Panel("").Header("Messages"));
    }

    public void Render(string gameText, string messages, string stats) {
        var gamePanel = new Panel(gameText).Header("Game").BorderColor(Color.Green);
        gamePanel.Border = BoxBorder.Ascii;

        var statBlock = new Panel(stats).Header("Stats").BorderColor(Color.Red);
        statBlock.Border = BoxBorder.Ascii;

        var messagePanel = new Panel(messages).Header("Messages").BorderColor(Color.Yellow);
        messagePanel.Border = BoxBorder.Ascii;
        messagePanel.Width = width;

        _layout["GameArea"].Update(gamePanel);
        _layout["StatBlock"].Update(statBlock);
        _layout["Messages"].Update(messagePanel);
    }
}

public class UserInterface
{

    public void userInterface()
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
