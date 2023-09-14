using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MedGame.UI.Mobile.Interfaces
{
    public interface IAudioService
    {
        int GetCurrentTimeStamp();
        int GetFileDurationTime();
        bool IsPlaying();
        Task PlayAudioFile(string fileName);
        void StopAudioFile();
    }
}
