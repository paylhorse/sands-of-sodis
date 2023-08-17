using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.UI; // For UI elements

public class DialogueInterface : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox; // Assign in Unity Inspector
    [SerializeField] private Text dialogueText; // Assign in Unity Inspector
    [SerializeField] private Text characterNameText; // Assign in Unity Inspector
    [SerializeField] private Image characterPortrait; // Assign in Unity Inspector

    private Queue<object> dialogueSequence = new Queue<object>();

    public void DisplayDialogue(string dialogueToml)
    {
        // Process the dialogue TOML via LuaBackbone to get the dialogue sequence
        DynValue luaDialogueSequence = LuaBackbone.Instance.luaData.Call(LuaBackbone.Instance.luaData.Globals["DialogueParser.parse"], dialogueToml);
        
        // Convert DynValue sequence into a C# Queue for easier processing
        foreach (var dialogue in luaDialogueSequence.Table.Values)
        {
            dialogueSequence.Enqueue(dialogue);
        }

        // Begin displaying the dialogue
        // ShowNextDialogue();
    }

    // private void ShowNextDialogue()
    // {
    //     if (dialogueSequence.Count == 0)
    //     {
    //         EndDialogue();
    //         return;
    //     }
    //
    //     var node = dialogueSequence.Dequeue();
    //     string type = node.Table.Get("type").String;
    //
    //     switch (type)
    //     {
    //         case "dialogue":
    //             DisplayDialogueNode(node);
    //             break;
    //
    //         case "command":
    //             ExecuteCommand(node.Table.Get("action").String, node.Table.Get("parameter").String);
    //             ShowNextDialogue(); // Immediately go to the next dialogue after executing the command
    //             break;
    //
    //         // Add more case checks for other node types...
    //
    //         default:
    //             Debug.LogError("Unknown dialogue node type: " + type);
    //             break;
    //     }
    // }
    //
    // private void DisplayDialogueNode(DynValue dialogueNode)
    // {
    //     string character = dialogueNode.Table.Get("character").String;
    //     string line = dialogueNode.Table.Get("line").String;
    //     int expression = (int)dialogueNode.Table.Get("expression").Number;
    //
    //     // Activate the dialogue box if it's not active
    //     if (!dialogueBox.activeInHierarchy)
    //         dialogueBox.SetActive(true);
    //
    //     // Set the character name, dialogue line, and character portrait
    //     characterNameText.text = character;
    //     dialogueText.text = line;
    //     characterPortrait.sprite = Resources.Load<Sprite>($"Faces/{character}/{expression}");
    //
    //     // Wait for user input to proceed, you might use a button press or mouse click for this
    // }
    //
    // private void EndDialogue()
    // {
    //     // Close the dialogue box and reset any other state if needed
    //     dialogueBox.SetActive(false);
    // }
    //
    // private void ExecuteCommand(string action, string parameter)
    // {
    //     // Handle the different command actions as discussed earlier
    //     switch (action)
    //     {
    //         case "playAnimation":
    //             // Handle animation playing
    //             break;
    //
    //         case "playSound":
    //             // Handle sound playing
    //             break;
    //
    //         // ... other cases ...
    //
    //         default:
    //             Debug.LogError("Unknown command action: " + action);
    //             break;
    //     }
    // }

    // Add more methods to handle other UI interactions, animations, etc. as needed

}
