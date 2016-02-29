using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine.EventSystems;

public class AssignActionsToInputs : MonoBehaviour {

	[SerializeField] protected EventTrigger leftTrigger;
	[SerializeField] protected EventTrigger rightTrigger;
	[SerializeField] protected EventTrigger jumpTrigger;
	[SerializeField] protected EventTrigger shieldTrigger;

	void Awake(){
		this.StartSafeCoroutine(WaitOnPlayerInstantiation());
	}

	IEnumerator WaitOnPlayerInstantiation(){
		while (PlayerController.Instance == null){
			yield return null;
		}

		AssignActions();
	}

	void AssignActions(){
		AssignLeftActions();
		AssignRightActions();
		AssignUpActions();
		AssignDownActions();
	}

	void AssignAllActions(System.Action<BaseEventData> pressedAction,
						  System.Action<BaseEventData> releasedAction,
						  ref EventTrigger trigger)
	{
		EventTrigger.Entry entry1 = new EventTrigger.Entry();

		entry1.eventID = EventTriggerType.PointerDown;
		entry1.callback.AddListener(eventData => pressedAction.Invoke(eventData));

		trigger.triggers.Add(entry1);

		EventTrigger.Entry entry2 = new EventTrigger.Entry();

		entry2.eventID = EventTriggerType.PointerUp;
		entry2.callback.AddListener(eventData => releasedAction.Invoke(eventData));

		trigger.triggers.Add(entry2);

		EventTrigger.Entry entry3 = new EventTrigger.Entry();

		entry3.eventID = EventTriggerType.PointerExit;
		entry3.callback.AddListener(eventData => releasedAction.Invoke(eventData));

		trigger.triggers.Add(entry3);
	}

	void AssignLeftActions(){
		AssignAllActions(
			eventData => PlayerController.Instance.InputPressedLeft(),
			eventData => PlayerController.Instance.InputReleasedLeft(),
			ref leftTrigger
		);
	}

	void AssignRightActions(){
		AssignAllActions(
			eventData => PlayerController.Instance.InputPressedRight(),
			eventData => PlayerController.Instance.InputReleasedRight(),
			ref rightTrigger
		);
	}

	void AssignDownActions(){
		AssignAllActions(
			eventData => PlayerController.Instance.InputPressedDown(),
			eventData => PlayerController.Instance.InputReleasedDown(),
			ref shieldTrigger
		);
	}

	void AssignUpActions(){
		AssignAllActions(
			eventData => PlayerController.Instance.InputPressedUp(),
			eventData => PlayerController.Instance.InputReleasedUp(),
			ref jumpTrigger
		);
	}
}
