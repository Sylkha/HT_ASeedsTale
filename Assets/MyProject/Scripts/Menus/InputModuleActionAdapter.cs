using UnityEngine;
using InControl;


[RequireComponent(typeof(InControlInputModule))]
public class InputModuleActionAdapter : MonoBehaviour
{
    public class InputModuleActions : PlayerActionSet
    {
        public PlayerAction Submit;
        public PlayerAction Cancel;
        public PlayerAction Left;
        public PlayerAction Right;
        public PlayerAction Up;
        public PlayerAction Down;
        public PlayerTwoAxisAction Move;


        public InputModuleActions()
        {
            Submit = CreatePlayerAction("Submit");
            Cancel = CreatePlayerAction("Cancel");
            Left = CreatePlayerAction("Move Left");
            Right = CreatePlayerAction("Move Right");
            Up = CreatePlayerAction("Move Up");
            Down = CreatePlayerAction("Move Down");
            Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
        }
    }


    InputModuleActions actions;


    void OnEnable()
    {
        CreateActions();

        var inputModule = GetComponent<InControlInputModule>();
        if (inputModule != null)
        {
            inputModule.SubmitAction = actions.Submit;
            inputModule.CancelAction = actions.Cancel;
            inputModule.MoveAction = actions.Move;
        }
    }


    void OnDisable()
    {
        DestroyActions();
    }


    void CreateActions()
    {
        actions = new InputModuleActions();

        actions.Submit.AddDefaultBinding(InputControlType.Action1);
        actions.Submit.AddDefaultBinding(Key.Return);
        actions.Submit.AddDefaultBinding(Key.E);

        actions.Cancel.AddDefaultBinding(InputControlType.Action2);
        actions.Cancel.AddDefaultBinding(Key.Escape);

        actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Up.AddDefaultBinding(Key.W);

        actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Down.AddDefaultBinding(Key.S);

        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Left.AddDefaultBinding(Key.A);

        actions.Right.AddDefaultBinding(InputControlType.RightStickRight);
        actions.Right.AddDefaultBinding(Key.RightArrow);
        actions.Right.AddDefaultBinding(Key.D);
    }


    void DestroyActions()
    {
        actions.Destroy();
    }
}
