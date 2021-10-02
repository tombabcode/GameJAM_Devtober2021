using Microsoft.Xna.Framework.Input;

namespace GameJAM_Devtober2021.System.Controllers {
    public class InputController {

        private KeyboardState _curK;
        private KeyboardState _preK;

        private MouseState _curM;
        private MouseState _preM;

        public void Update( ) {
            _preK = _curK;
            _curK = Keyboard.GetState( );

            _preM = _curM;
            _curM = Mouse.GetState( );
        }

        public int MouseDiffX => _curM.X - _preM.X;
        public int MouseDiffY => _curM.Y - _preM.Y;

        public bool IsKeyPressed(Keys key) => _curK.IsKeyDown(key);
        public bool IsKeyPressedOnce(Keys key) => _curK.IsKeyDown(key) && _preK.IsKeyUp(key);

        public bool IsLMBPressed( ) => _curM.LeftButton == ButtonState.Pressed;
        public bool IsMMBPressed( ) => _curM.MiddleButton == ButtonState.Pressed;
        public bool IsRMBPressed( ) => _curM.RightButton == ButtonState.Pressed;

        public bool IsLMBPressedOnce( ) => _curM.LeftButton == ButtonState.Pressed && _preM.LeftButton != ButtonState.Pressed;
        public bool IsMMBPressedOnce( ) => _curM.MiddleButton == ButtonState.Pressed && _preM.MiddleButton != ButtonState.Pressed;
        public bool IsRMBPressedOnce( ) => _curM.RightButton == ButtonState.Pressed && _preM.RightButton != ButtonState.Pressed;

        public bool HasScrolledDown( ) => _curM.ScrollWheelValue < _preM.ScrollWheelValue;
        public bool HasScrolledUp( ) => _curM.ScrollWheelValue > _preM.ScrollWheelValue;
        public bool HasScrolled( ) => _curM.ScrollWheelValue != _preM.ScrollWheelValue;

    }
}
