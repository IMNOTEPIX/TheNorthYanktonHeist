using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Scaleforms
{
    using GTA;
    using GTA.Native;
    using System.Collections.Generic;
    using TheNorthYanktonHeist.Funcs;

    /// <summary>
    /// Single instructional button displayed at bottom of screen
    /// </summary>
    public class InstructionalButton
    {
        public Control Control { get; set; }
        public string Text { get; set; }

        public InstructionalButton(Control control, string text)
        {
            Control = control;
            Text = text;
        }

        /// <summary>
        /// Get button ID for scaleform (auto-detects keyboard vs controller)
        /// </summary>
        public string GetButtonId()
        {
            return Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, (int)Control, true);
        }
    }

    /// <summary>
    /// Manages and displays instructional buttons at bottom of screen
    /// </summary>
    public class InstructionalButtons
    {
        private Scaleform scaleform;
        private List<InstructionalButton> buttons;
        private bool needsUpdate;
        private int lastInputMethod;

        public List<InstructionalButton> Buttons => buttons;

        public InstructionalButtons()
        {
            scaleform = Scaleform.RequestMovie("instructional_buttons");
            buttons = new List<InstructionalButton>();
            needsUpdate = true;
            lastInputMethod = -1;
        }

        /// <summary>
        /// Add a button
        /// </summary>
        public void Add(Control control, string text)
        {
            buttons.Add(new InstructionalButton(control, text));
            needsUpdate = true;
        }

        /// <summary>
        /// Add a button
        /// </summary>
        public void Add(InstructionalButton button)
        {
            buttons.Add(button);
            needsUpdate = true;
        }

        /// <summary>
        /// Remove a button by control
        /// </summary>
        public void Remove(Control control)
        {
            buttons.RemoveAll(b => b.Control == control);
            needsUpdate = true;
        }

        /// <summary>
        /// Clear all buttons
        /// </summary>
        public void Clear()
        {
            buttons.Clear();
            needsUpdate = true;
        }

        /// <summary>
        /// Update the scaleform with current buttons
        /// </summary>
        private void Update()
        {
            scaleform.CallFunction("CLEAR_ALL");
            scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", false);
            scaleform.CallFunction("CREATE_CONTAINER");

            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                scaleform.CallFunction("SET_DATA_SLOT", i, button.GetButtonId(), button.Text);
            }

            scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
            needsUpdate = false;
        }

        /// <summary>
        /// Draw the buttons
        /// </summary>
        public void Draw()
        {
            // Check if input method changed (keyboard <-> controller)
            int currentInputMethod = Function.Call<int>((Hash)0xA571D46727E2B718, 2);  // _GET_LAST_INPUT_METHOD

            if (currentInputMethod != lastInputMethod)
            {
                needsUpdate = true;
                lastInputMethod = currentInputMethod;
            }

            // Update if needed
            if (needsUpdate)
            {
                Update();
            }

            // Draw
            scaleform.Render2D();
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            scaleform?.Dispose();
        }
    }
}
