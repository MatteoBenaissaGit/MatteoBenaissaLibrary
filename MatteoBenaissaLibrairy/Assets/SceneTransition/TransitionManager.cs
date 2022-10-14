namespace SceneTransition
{
    using UnityEngine;

    public class TransitionManager : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Transition type")] 
        
        [SerializeField, Tooltip("Choice of the transition type")]
        private TransitionType _transitionType;

        [Header("References")] 
        
        [Tooltip("Reference the fade image")]
        public Animator FadeImageAnimator;

        [Tooltip("Reference the size image")]
        public Animator SizeImageAnimator;

        #endregion

        #region Methods

        private void Start()
        {
            FadeImageAnimator.gameObject.SetActive(_transitionType == TransitionType.Fade);
            SizeImageAnimator.gameObject.SetActive(_transitionType == TransitionType.Size);
        }

        public void LaunchTransition()
        {
            const string transitionInTriggerName = "TransitionIn";
            
            switch (_transitionType)
            {
                case TransitionType.Fade:
                    FadeImageAnimator.SetTrigger(transitionInTriggerName);
                    break;
                case TransitionType.Size:
                    SizeImageAnimator.SetTrigger(transitionInTriggerName);
                    break;
            }
        }

        #endregion
    }
}