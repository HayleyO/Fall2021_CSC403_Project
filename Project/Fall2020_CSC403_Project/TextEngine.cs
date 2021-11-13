using MyGameLibrary.Shop;
using MyGameLibrary.Story;
using MyGameLibrary.Character;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Fall2020_CSC403_Project
{
    public partial class TextEngine : Form
    {
        private Story Story { get; set; }
        private Stack<Option> Options = new Stack<Option>();
        private Stack<Option> TempOptions = new Stack<Option>();
        private List<Option> options = new List<Option>();

        public double ForegroundImage_Xscale { get; set; }
        public double ForegroundImage_Yscale { get; set; }
        public double ForegroundImage_AspectRatio { get; set; }
        public int originalHeight { get; set; }
        public int originalWidth { get; set; }
        private bool isShop { get; set; } = false; 
        public TextEngine()
        {
            this.Story = new Story("\\data\\story\\", "Menu.txt"); //Read in main story
            #region Initalize shops and items
            Item.initializeAllItems();
            Item.initializeHannahShop();
            Item.initializeHayleyShop();
            #endregion
            InitializeComponent();
            //Start parsing things
            HandleMarkup(Story.GetNextLine());
        }

        public void NewBackground(Image newBackground)
        {
            Image oldBGImageNormalPanel = NormalPanel.BackgroundImage;
            Image oldBGImageOptionsPanel = OptionsPanel.BackgroundImage;
            NormalPanel.BackgroundImage = newBackground;
            OptionsPanel.BackgroundImage = newBackground;
            oldBGImageNormalPanel.Dispose();
            oldBGImageOptionsPanel.Dispose();
        }

        public void NewForeground(Image newImage)
        {
            Image oldFGImage = ForegroundImage.BackgroundImage;
            ForegroundImage.BackgroundImage = newImage;
            oldFGImage.Dispose();
        }

        public void ChangeText(string newText)
        {
            Textbox.contents = newText;
            Textbox.Refresh();
        }

        public void ForegroundVisibile(bool hide)
        {
            ForegroundImage.Visible = hide;
        }
        public void TextboxVisible(bool hide)
        {
            Textbox.Visible = hide;
        }

        private void TextEngine_KeyPress(object sender, KeyEventArgs e)
        {
            if (this.Options.Count != 0)
            {
                if (e.KeyCode == Keys.Down && Options.Count > 1)
                {
                    Options.Peek().OptionFocused = false; //Top option unfocused
                    TempOptions.Push(Options.Pop()); //Put it in the temp stack
                    Options.Peek().OptionFocused = true; //New top option focused
                }
                if (e.KeyCode == Keys.Up && TempOptions.Count > 0)
                {
                    Options.Peek().OptionFocused = false; //Top option unfocused
                    Options.Push(TempOptions.Pop()); //Put it in the temp stack
                    Options.Peek().OptionFocused = true; //New top option focused
                }
                if (e.KeyCode == Keys.Enter)
                {
                    //DO MARKUP OF OPTION                    
                    List<Option> options = new List<Option>();
                    Option focusedOption = new Option("uh oh", "#CT ERROR, OPTION NOT FOUND");
                    foreach (Option option in TempOptions)
                    {
                        if (option.OptionFocused)
                        {
                            focusedOption = option;
                        }
                        options.Add(option);
                    }
                    foreach (Option option in Options)
                    {
                        if (option.OptionFocused)
                        {
                            focusedOption = option;
                        }
                        options.Add(option);
                    }
                    if (isShop && string.Equals(focusedOption.OptionBackendMarkup, "#E"))
                    {
                        isShop = false;
                    }
                    #region Clear all the visible options
                    TempOptions.Clear();
                    Options.Clear();
                    this.RemoveOptions(options);
                    #endregion
                    Console.WriteLine(focusedOption.OptionBackendMarkup);
                    Story.CurrentStoryText.AddFirst(focusedOption.OptionBackendMarkup); //Add whatever the option markup was to story to handle next
                    HandleMarkup(Story.GetNextLine());
                }
            }
            else
            {
                HandleMarkup(Story.GetNextLine());
            }
        }

        private async void HandleMarkup(string line)
        { 
            string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            _filePath = Directory.GetParent(Directory.GetParent(_filePath).FullName).FullName;
            switch (Story.Current_Action)
            {
                case Markup.ChangeText:
                    //line is plain text (ex: #CT Hello)
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        HandleMarkup(Story.GetNextLine());
                    }
                    else
                    {
                        this.ChangeText(line);
                    }                    
                    break;
                case Markup.Pause:
                    // line is: how long to pause in seconds
                    await Task.Delay(int.Parse(line) * 1000);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.ChangeBackgroundImage:
                    //line is: image_location image_name text (ex: #CB \\data\\background\\ hannah_store.jpg We've changed the background)
                    List<string> backgroundInfo = line.Split(' ').ToList();
                    Image backgroundImage = Image.FromFile(_filePath + backgroundInfo[0] + backgroundInfo[1]);
                    this.NewBackground(backgroundImage);
                    backgroundInfo.RemoveAt(0);
                    backgroundInfo.RemoveAt(0);
                    string newLineBG = string.Join(" ", backgroundInfo);
                    if (string.IsNullOrWhiteSpace(newLineBG))
                    {
                        HandleMarkup(Story.GetNextLine());
                    }
                    else
                    {
                        this.ChangeText(newLineBG);
                    }
                    break;
                case Markup.ChangeForegroundImage:
                    //line is: image_location image_name text (ex: #CF \\data\\foreground\\ player.png We've changed the foreground)
                    List<string> foregroundInfo = line.Split(' ').ToList();
                    Image foregroundImage = Image.FromFile(_filePath + foregroundInfo[0] + foregroundInfo[1]);
                    this.NewForeground(foregroundImage);
                    foregroundInfo.RemoveAt(0);
                    foregroundInfo.RemoveAt(0);
                    string newLineFG = string.Join(" ", foregroundInfo);
                    if (string.IsNullOrWhiteSpace(newLineFG))
                    {
                        HandleMarkup(Story.GetNextLine());
                    }
                    else
                    {
                        this.ChangeText(newLineFG);
                    }
                    break;
                case Markup.ChangeBackgroundAndForegroundImage:
                    //line is: background_location background_name foreground_location foreground_name text
                    List<string> backForeInfo = line.Split(' ').ToList();
                    Image backForeBackgroundImage = Image.FromFile(_filePath + backForeInfo[0] + backForeInfo[1]);
                    this.NewBackground(backForeBackgroundImage);
                    Image backForeForegroundImage = Image.FromFile(_filePath + backForeInfo[2] + backForeInfo[3]);
                    this.NewForeground(backForeForegroundImage);
                    backForeInfo.RemoveAt(0);
                    backForeInfo.RemoveAt(0);
                    backForeInfo.RemoveAt(0);
                    backForeInfo.RemoveAt(0);
                    string newLineBGFG = string.Join(" ", backForeInfo);
                    if (string.IsNullOrWhiteSpace(newLineBGFG))
                    {
                        HandleMarkup(Story.GetNextLine());
                    }
                    else
                    {
                        this.ChangeText(newLineBGFG);
                    }                    
                    break;
                case Markup.HideForeground:
                    // line is: optional text
                    this.ForegroundVisibile(false);
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        HandleMarkup(Story.GetNextLine());
                    }
                    else
                    {
                        this.ChangeText(line);
                    }
                    break;
                case Markup.ShowForeground:
                    // line is: optional text
                    this.ForegroundVisibile(true);
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        HandleMarkup(Story.GetNextLine());
                    }
                    else
                    {
                        this.ChangeText(line);
                    }
                    break;
                case Markup.HideTextBox:
                    // line is: nothing, this is just a signal to hide the textbox
                    this.TextboxVisible(false);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.ShowTextbox:
                    // line is: nothing, this is just a signal to show the textbox
                    this.TextboxVisible(true);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.Options:
                    //line is: Option 1, #A ID] Option 2, #A ID] Exit, #CT] text
                    // (ex: #O Option 1,#CT Oh, you selected that] Option 2,#CT You've selected this] Oh look, it's the options page)
                    //Note: make sure there is no space between ,#
                    
                    isShop = false; // Default to believe no shop is running
                    if(Equals(line.Substring(0, 2), "S ")) // If options have a shop flag, then there is a shop
                    {
                        isShop = true;
                        line = line.Substring(2); // Split of the shop flag from the rest of the options
                    }
                    List<string> optionsInfo = line.Split(']').ToList();

                    string newLineOptions = optionsInfo[optionsInfo.Count - 1];
                    optionsInfo.RemoveAt(optionsInfo.Count - 1);

                    options = new List<Option>();
                    foreach (string info in optionsInfo)
                    {
                        string newInfo = info.Replace(",#", "-");
                        string[] newOption = newInfo.Split('-');
                        options.Add(new Option(newOption[0], '#' + newOption[1]));
                    }
                    this.DisplayOptions(options); //Display options
                    // Make sure that the foreground image is at the correct location in the options panel
                    this.OptionsPanel.Controls[1].Location = new Point((int)(ForegroundImage_Xscale * ClientRectangle.Size.Width), (int)(ForegroundImage_Yscale * Height));
                    options.Reverse(); //Start at end
                    foreach (Option option in options)
                    {
                        option.OptionFocused = false; //Unfocus all options
                        this.Options.Push(option);
                    }
                    this.Options.Peek().OptionFocused = true; //Focus the top option 
                    this.ChangeText(newLineOptions);
                    options.Reverse(); //This is needs to be set back to the way it was because of accessing ambiguity
                    break;
                case Markup.AddToWallet:
                    //line is: amount as int
                    Inventory.depositMoney(int.Parse(line));
                    UpdateBalance(true);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.HideWallet:
                    //line is: nothing, just used to hide the wallet when not needed
                    UpdateBalance(false);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.AddItemToInventory:
                    // line is: id           
                    string nameAdd = Enum.GetName(typeof(Items), int.Parse(line));
                    Items identifierAdd = (Items)Enum.Parse(typeof(Items), nameAdd);
                    Item itemAdd = Item.allItems[(int)identifierAdd];
                    if(itemAdd.ItemPrice <= Inventory.walletBalance)
                    {
                        // if the options are a shop, refresh the shop options after buying an item
                        if (isShop)
                        {
                            if (Item.HannahShop.Products.ContainsKey((int)identifierAdd))
                            {
                                Item.HannahShop.purchase(identifierAdd);
                                Story.CurrentStoryText.AddFirst("#S1");
                            }
                            else
                            {
                                Item.HayleyShop.purchase(identifierAdd);
                                Story.CurrentStoryText.AddFirst("#S2");
                            }
                        }
                    }
                    else
                    {
                        if (Item.HannahShop.Products.ContainsKey((int)identifierAdd))
                        {
                            Story.CurrentStoryText.AddFirst("#S1");
                        }
                        else
                        {
                            Story.CurrentStoryText.AddFirst("#S2");
                        }
                    }
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.GiveItem:
                    // line is: characterID itemID
                    // uses the item and changes the character's love score depending on the item

                    // get character and item IDs
                    string[] IDs = line.Split(' '); 
                    string character = Enum.GetName(typeof(CharacterID), int.Parse(IDs[0]));
                    CharacterID ID = (CharacterID)Enum.Parse(typeof(CharacterID), character);
                    string nameGive = Enum.GetName(typeof(Items), int.Parse(IDs[1]));
                    Items identifierGive = (Items)Enum.Parse(typeof(Items), nameGive);

                    Inventory.useItem(identifierGive); //mark the item as used
                    string response = CharacterCollection.CharacterDictionary[ID].LoveUpdate(identifierGive);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        HandleMarkup(Story.GetNextLine());
                    }
                    else
                    {
                        response = CharacterCollection.CharacterDictionary[ID].CharacterName + ": " + response;
                        this.ChangeText(response);
                    }
                    break;
                case Markup.ReadInNewStory:
                    // line is: story_location story_name
                    string[] newStoryInfo = line.Split(' ');
                    Story.ChangeStory(newStoryInfo[1], newStoryInfo[0]);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.ViewInventory:
                    // line is: empty, just display inventory
                    //for items in current inventory create options where markup is #CT item_description
                    //to leave inventory handle pressing escape? press escape maybe to view inventory and then escape to exit?
                    break;
                case Markup.CheckPromPosal:
                    // line is: empty, just checking existing characters
                    // Get the character you like the most
                    bool foundCharacter = false;
                    Character maxLoveCharacter = null;
                    while (!foundCharacter && CharacterCollection.CharacterDictionary.Values.Count > 0)
                    {
                        maxLoveCharacter = CharacterCollection.CharacterDictionary.Values.Aggregate((char1, char2) => (char1.LoveScore > char2.LoveScore) ? char1 : char2);
                        if (maxLoveCharacter.IsDead == true)
                        {
                            CharacterCollection.CharacterDictionary.Remove((CharacterID)maxLoveCharacter.ID);
                            maxLoveCharacter = null;
                        }
                        else
                        {
                            foundCharacter = true;
                        }                       
                    }
                    if(maxLoveCharacter != null)
                    {
                        Story.ChangeStory(maxLoveCharacter.PromName, maxLoveCharacter.PromLocation);
                    }
                    else
                    {
                        Story.ChangeStory("Bad_End.txt", "\\data\\story\\");
                    }
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.CheckIfDead:
                    // line is: characterID you're checking
                    string characterNameIfDead = Enum.GetName(typeof(CharacterID), int.Parse(line));
                    CharacterID characterIDIfDead = (CharacterID)Enum.Parse(typeof(CharacterID), characterNameIfDead);
                    Character characterIfDead = CharacterCollection.CharacterDictionary[characterIDIfDead];
                    if(characterIfDead.IsDead)
                    {
                        string[] duplicateStoryIfDead = new string[this.Story.CurrentStoryText.Count];
                        this.Story.CurrentStoryText.CopyTo(duplicateStoryIfDead, 0);
                        foreach (string storyLine in duplicateStoryIfDead)
                        {
                            //Until you reach the "SKIP TO DEAD" (aka [STD] text marker) clear out the rest of the dialogue
                            this.Story.CurrentStoryText.RemoveFirst();
                            if (string.Equals(storyLine.Trim(), "[STD]"))
                            {
                                break;
                            }
                        }
                    }
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.CheckThresholdsForTree:
                    // line is: characterID you're checking
                    //If greater than or equal to max of items and not already dated
                    string characterName = Enum.GetName(typeof(CharacterID), int.Parse(line));
                    CharacterID characterID = (CharacterID)Enum.Parse(typeof(CharacterID), characterName);
                    Character characterTree = CharacterCollection.CharacterDictionary[characterID];
                    if(!characterTree.Dated && characterTree.LoveScore >= 100)
                    {
                        string[] duplicateStory = new string[this.Story.CurrentStoryText.Count];
                        this.Story.CurrentStoryText.CopyTo(duplicateStory, 0);
                        foreach (string storyLine in duplicateStory)
                        {
                            //Until you reach the "SKIP TO" (aka [ST] text marker) clear out the rest of the dialogue
                            this.Story.CurrentStoryText.RemoveFirst();
                            if (string.Equals(storyLine.Trim(), "[ST]"))
                            {
                                break;
                            }                            
                        }
                        //Read in the date
                        this.Story.ChangeStory(characterTree.DateName, characterTree.DateLocation, insert: true);
                        characterTree.Dated = true;
                    }
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.ExitOptions:
                    // line is: #E
                    // allows the user to exit the shop options by changing isShop to false
                    isShop = false;
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.HannahShopOptions:
                    UpdateBalance(true);
                    // line is: #S1
                    // creates a string containing the available items to be displayed as options
                    string shop1OptionString = "#O S ";
                    if(this.Options.Count == 0)
                    {
                        foreach (int itemID in Item.HannahShop.Products.Keys)
                        {
                            shop1OptionString += (Item.HannahShop.Products[itemID].ItemName + ": $" + Item.HannahShop.Products[itemID].ItemPrice + ",#A " + itemID + "] ");
                        }
                    }
                    shop1OptionString += " Exit,#E]  Hannah: Hello, please buy something.";
                    Story.CurrentStoryText.AddFirst(shop1OptionString);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.HayleyShopOptions:
                    UpdateBalance(true);
                    // line is: #S2
                    // creates a string containing the available items to be displayed as options
                    string shop2OptionString = "#O S ";
                    foreach (int itemID in Item.HayleyShop.Products.Keys)
                    {
                        shop2OptionString += (Item.HayleyShop.Products[itemID].ItemName + ": $" + Item.HayleyShop.Products[itemID].ItemPrice + ",#A " + itemID + "] ");
                    }
                    shop2OptionString += " Exit,#E] Hayley: Check it.";
                    Story.CurrentStoryText.AddFirst(shop2OptionString);
                    HandleMarkup(Story.GetNextLine());
                    break;
                case Markup.GiftOptions:
                    // line is: #GO [character ID]
                    // creates a string containing the available items in the users inventory to be displayed as gift options
                    int charID = int.Parse(line[0].ToString());
                    string giftOptions = "#O ";
                    foreach(Items itemID in Inventory.Contents.Keys)
                    {
                        if (!Inventory.Contents[itemID].used)
                        {
                            giftOptions += (Inventory.Contents[itemID].ItemName + ",#G " + charID + " " + (int)itemID + "]");
                        }
                    }
                    giftOptions += " Exit,#CT ] Select your gift!";
                    Story.CurrentStoryText.AddFirst(giftOptions);
                    HandleMarkup(Story.GetNextLine());
                    break;
            }
        }

        private void ResizeHandler(object sender, EventArgs e)
        {
            SuspendLayout();
            if (ForegroundImage_AspectRatio != 0)
            {
                ForegroundImage.Size = new Size((int)(ClientRectangle.Size.Height * ForegroundImage_AspectRatio), ClientRectangle.Size.Height);
            }
            if (ForegroundImage_Xscale != 0 && ForegroundImage_Yscale != 0)
            {
                ForegroundImage.Location = new Point((int)(ForegroundImage_Xscale * ClientRectangle.Size.Width), (int)(ForegroundImage_Yscale*Height));
            }
            if (originalHeight != 0 && Height != 0 && Width != 0)
            {
                double heightScaling = Height / (double)originalHeight;
                double widthScaling = Width / (double)originalWidth;
                Textbox.Height = (int)(heightScaling * 80);
                Textbox.Width = Width;
                BalanceLabel.Height = 30 * (int) heightScaling;
                BalanceLabel.Width = 70 * (int) widthScaling;
                BalanceLabel.Location = new Point(ClientRectangle.Width-BalanceLabel.Width, 0);
                BalanceLabel.Font = new Font("Verdana Pro",(int)(heightScaling*16));
                Textbox.Font = new Font(Textbox.Font.FontFamily, (int)(heightScaling * 12));
                int location_Y = 50;
                int optionNumber = 0;
                foreach(Option option in options) 
                {
                    optionNumber++;
                    option.OptionLabel.Size = new Size((int)(widthScaling * 150), (int)(heightScaling * 25));
                    option.OptionLabel.Font = new Font(option?.OptionLabel.Font.FontFamily, (int)Math.Ceiling(heightScaling * 8));
                    if(optionNumber == 6)
                    {
                        location_Y = 50;
                        option.OptionLabel.Location = new Point(40 + option.OptionLabel.Width, (int)(location_Y * heightScaling));
                    }
                    else if(optionNumber>6)
                    { 
                        option.OptionLabel.Location = new Point(40 + option.OptionLabel.Width, (int)(location_Y * heightScaling));
                    }
                    else
                    {
                        option.OptionLabel.Location = new Point(20, (int)(location_Y * heightScaling));
                    }
                    location_Y += 40;
                }
            }
            ResumeLayout();
        }

        private void TextEngine_Load(object sender, EventArgs e)
        {

        }

        private void TextEngine_Load_1(object sender, EventArgs e)
        {

        }
    }
}
