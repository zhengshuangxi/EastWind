using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string anim;
    public string audio;
    public string waiterText;
    public bool displayDragonText = true;
    public string dragonText;
    public List<string> answerContents;

    public static Dialogue dialogueTwo = new Dialogue
    (
    "Ask_2",
    "Restaurant/starters",
    "Here is the menu. We have many choices\nfor the starters, the main course, desserts\nand drinks. What would you like to have\nfor starters?",
    "starter 开胃小吃 salad 沙拉 soup 汤\n回答：I would like…",
    new List<string>() { "A.Salad", "B.Soup" }
    );

    public static Dialogue dialogueThree = new Dialogue
    (
    "Ask_1",
    "Restaurant/maincourse",
    "What would you like to have for the main course?",
    "steak 肉排 pasta 意大利面 hamburger 汉堡包\n回答：I would like…",
    new List<string>() { "A.Steak", "B.Pasta", "C.Hamburger" }
    );

    public static Dialogue dialogueFour = new Dialogue
    (
    "Ask_2",
    "Restaurant/dessert",
    "What would you like to have for the dessert?",
    "dessert 甜品 回答：I would like…",
    new List<string>() { "A.Ice Cream", "B.Cake", "C.Chocolate", "D.Cookies", "E.Pudding" }
    );

    public static Dialogue dialogueFive = new Dialogue
    (
    "Ask_1",
    "Restaurant/drink",
    "Would you like something to drink?",
    "回答：I would like…",
    new List<string>() { "A.Coffee, Milk, Tea, Water", "B.Juice", "C.Coco cola, Wine" }
    );

    public static Dialogue dialogueSix = new Dialogue
    (
    "Ask_2",
    "Restaurant/else",
    "Would you like anything else, sir?",
    "回答：I would like…",
    new List<string>() { "A.No, thanks!", "B.Maybe later." }
    );

    public static Dialogue dialogueSalad = new Dialogue
    (
    "Ask_1",
    "Restaurant/whatsalad",
    "What salad would you like?",
    "回答：I would like…",
    new List<string>() { "A.Caesar Salad", "B.Mixed Vegetables Salad", "C.Seafood Salad With Fruit", "D.Tuna Fish Salad", "E.Smoked Salmon Salad" }
    );

    public static Dialogue dialogueSaladDressing = new Dialogue
    (
    "Ask_2",
    "Restaurant/whatsaladdressing",
    "What salad dressing would you like?",
    "回答：I would like…",
    new List<string>() { "A.Caesar", "B.Thousand Island", "C.Vinaigrette", "D.Ranch" }
    );

    public static Dialogue dialogueSoup = new Dialogue
    (
    "Ask_1",
    "Restaurant/whatsoup",
    "What soup would you like?",
    "回答：I would like…",
    new List<string>() { "A.Cream Mushroom Soup", "B.Traditional Tomato Soup", "C.French Onion Soup", "D.Borsch" }
    );

    public static Dialogue dialogueSteak = new Dialogue
    (
    "Ask_2",
    "Restaurant/steak",
    "How would you like your steak?",
    "回答：I would like…",
    new List<string>() { "A.Rare", "B.Medium", "C.Well-done" }
    );

    public static Dialogue dialoguePasta = new Dialogue
    (
    "Ask_1",
    "Restaurant/pasta",
    "What kind of pasta would you like?",
    "回答：I would like…",
    new List<string>() { "A.Spaghetti", "B.Bow Ties", "C.Shells", "D.Spirals", "E.Ravioli" }
    );

    public static Dialogue dialogueHamburger = new Dialogue
    (
    "Ask_2",
    "Restaurant/hamburger",
    "What meat would you like for your hamburger?",
    "回答：I would like…",
    new List<string>() { "A.Chicken", "B.Pork", "C.Beef", "D.Shrimp", "E.Bacon" }
    );

    public static Dialogue dialogueSauce = new Dialogue
    (
    "Ask_1",
    "Restaurant/sauce",
    "What sauce would you like?",
    "sauce 酱汁;调味汁 回答：I would like…",
    new List<string>() { "A.Black Pepper Sauce", "B.Red Wine Sauce", "C.Creamy Mushroom Sauce" }
    );

    public static Dialogue dialogueCoffe = new Dialogue
    (
    "Ask_2",
    "Restaurant/coffee",
    "Do you want it hot or cold?",
    "",
    new List<string>() { "A.Hot, please.", "B.Cold, please." },
    false
    );

    public static Dialogue dialogueJuice = new Dialogue
    (
    "Ask_1",
    "Restaurant/juice",
    "What juice would you like?",
    "",
    new List<string>() { "A.Applce", "B.Orange", "C.Grape", "D.Pine Apple", "E.Kiwi" },
    false
    );

    public Dialogue() { }

    public Dialogue(string anim, string audio, string waiterText, string dragonText, List<string> answerContents, bool displayDragonText = true)
    {
        this.anim = anim;
        this.audio = audio;
        this.waiterText = waiterText;
        this.displayDragonText = displayDragonText;
        this.dragonText = dragonText;
        this.answerContents = answerContents;
    }
}
