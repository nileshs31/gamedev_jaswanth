using UnityEngine;

#if UNITY_IOS
    using Unity.Notifications.iOS;
#elif UNITY_ANDROID
    using Unity.Notifications.Android;
#endif

using System;

public class NotificationManager : MonoBehaviour {
    private void Start() {


#if UNITY_ANDROID
        AndroidNotificationChannel channel = new AndroidNotificationChannel {
            Id = "generalNotifs_ttt",
            Name = "General Channel",
            Importance = Importance.High,
            Description = "Send notifications to remind the player to open the game!!",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        sendNotificationAndroid("taptaptap", "Hey Haven't seen u in a while", DateTime.Today.AddHours(12));
#elif UNITY_IOS
        sendNotificationIos("dailyDosage", "Shashiburi daana", "Hey come play the game", "remainder", DateTime.Today.AddHours(12));
#endif
    }
    // some public methods
#if UNITY_ANDROID
    public void sendNotificationAndroid(string title,string text,DateTime firetime) {
        AndroidNotification notification = new AndroidNotification {
            Title = title,
            Text = text,
            FireTime = firetime,
            //SmallIcon = "icon_0",
            //LargeIcon = "icon_1",
        };

        int id = AndroidNotificationCenter.SendNotification(notification, "generalNotifs_ttt");
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled) {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "generalNotifs_ttt");
        }
    }
#endif

#if UNITY_IOS
    public void sendNotificationIos(string id, string title,string body,string subtitle,DateTime fireTime) {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger() {
            TimeInterval = fireTime.TimeOfDay,
            Repeats = false
        };

        var notification = new iOSNotification() {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = id,
            Title = title,
            Body = body,
            Subtitle = subtitle,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.RemoveScheduledNotification(id);
        iOSNotificationCenter.ScheduleNotification(notification);
    }
#endif
}
