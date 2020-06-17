using UnityEngine;

namespace _Game._Scripts
{
    public interface IMouseInput
    {
        event MouseInput MovementInputEvent;
    }
    
    public delegate void MouseInput(Vector2 input);
}
