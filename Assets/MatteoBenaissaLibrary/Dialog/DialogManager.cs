using MatteoBenaissaLibrary.SingletonClassBase;
using UnityEngine;
using UnityEngine.UI;

namespace MatteoBenaissaLibrary.Dialog
{
    public class DialogManager : Singleton<DialogManager>
    {

        public TypeWriter TypeWriterText;
        public GameObject DialogUIGameObject;
        public Image PressButtonImage;

        public void ToggleDialog(bool setActive)
        {
            DialogUIGameObject.SetActive(setActive);
        }
    }
}