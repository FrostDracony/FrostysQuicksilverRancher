using SRML.SR.SaveSystem;
using SRML.SR.SaveSystem.Data;
using SRML.Console;
using System.Collections.Generic;

namespace FrostysQuicksilverRancher.Components
{
    class MochiWarning : SRBehaviour, ExtendedData.Participant
    {
        public int warning = 0;
        public double endingDay = -1;
        public MailDirector mailDir;
        public TimeDirector timeDir;
        public Dictionary<string, MailDirector.Mail> mails;

        public void Awake()
        {
            mailDir = SceneContext.Instance.MailDirector;
            timeDir = SceneContext.Instance.TimeDirector;
            mails = new Dictionary<string, MailDirector.Mail>();
        }

        public void Update()
        {
            if(timeDir.HasReached(endingDay)) //If it's time to end this conflict
            {
                Console.Log("Ending the Conflict");
                EndingTheConflict();
            }

        }

        public void ReadData(CompoundDataPiece piece)
        {
            Console.Log("Reading the Data");
            //Setting the fields
            warning = piece.GetValue<int>("Mochi's Warnings");
            endingDay = piece.GetValue<double>("Mochi's WarningEnd");
            mails = piece.GetValue<Dictionary<string, MailDirector.Mail>>("Mails");
        }

        public void WriteData(CompoundDataPiece piece)
        {
            Console.Log("Writing the Data");
            //Writing the data that's going to get stored
            piece.SetValue("Mochi's Warnings", warning);
            piece.SetValue("Mochi's WarningEnd", endingDay);
            piece.SetValue("Mails", mails);
        }

        public void IncrementWarning() //Function used to increment the warnings
        {
            if ( (warning + 1) >= 3) //if this is going to be the 4th warning
            {
                Console.Log("Mochi is VERY angry at you");
                mailDir.SendMail(MailDirector.Type.PERSONAL, "quicksilver_stolen_war"); //Send the conflict mail
                endingDay = timeDir.HoursFromNow(24 * 1); //Start the conflict from now on
                return; //Dont proceed with the code
            }
            
            warning += 1; //Else, increment the warnings
            mailDir.SendMail(MailDirector.Type.PERSONAL, "quicksilver_stolen_warning_" + warning); //And send the corresponding mail
        }

        void EndingTheConflict()
        {
            warning = 0;
            endingDay = -1;

            RemovingMails();

            mailDir.SendMail(MailDirector.Type.PERSONAL, "quicksilver_stolen_ending");
        }

        MailDirector.Mail GetMail(string key)
        {
            List<MailDirector.Mail> allMail = mailDir.model.allMail;
            for (int i = 0; i < allMail.Count; i++)
            {
                MailDirector.Mail mail = allMail[i];
                if(mail.key == key)
                {
                    return mail;
                }
            }

            return null;
        }

        void RemovingMails()
        {
            MailDirector.Mail mail1 = GetMail("quicksilver_stolen_warning_1");
            mailDir.model.allMail.Remove(mail1);
            mailDir.model.allMailDict[mail1] = mail1;

            MailDirector.Mail mail2 = GetMail("quicksilver_stolen_warning_2");
            mailDir.model.allMail.Remove(mail2);
            mailDir.model.allMailDict[mail2] = mail2;

            MailDirector.Mail mail3 = GetMail("quicksilver_stolen_warning_3");
            mailDir.model.allMail.Remove(mail3);
            mailDir.model.allMailDict[mail3] = mail3;

            MailDirector.Mail mailConflict = GetMail("quicksilver_stolen_war");
            mailDir.model.allMail.Remove(mailConflict);
            mailDir.model.allMailDict[mailConflict] = mailConflict;

            mailDir.model.MailListChanged();
        }

        public bool IsMochiAlarmed(int count) //The amount of quicksilvers needed to make Mochi suspicious of you
        {
            switch (warning) //For very warning, Mochi will get more and more attentive
            {
                case 1: return count >= 30;
                case 2: return count >= 15;
                case 3: return count >= 1;
                default: return false;
            }

        }

    }
}
