package com.example;

import android.content.Intent;
import android.os.RemoteException;
import com.iflytek.speech.*;

/**
 * Created with IntelliJ IDEA.
 * User: Administrator
 * Date: 14-2-18
 * Time: 下午4:28
 * To change this template use File | Settings | File Templates.
 */
public class SpeechHelper {

    private SpeechSynthesizer mTts;

    public SpeechHelper(android.content.Context context, String appid) {
        SpeechUtility.getUtility(context).setAppid(appid);
        mTts = new SpeechSynthesizer(context, mTtsInitListener);

        //setParam();
    }

    public int startSpeaking(String text) {
        int code = mTts.startSpeaking(text, mTtsListener);
        return code;
    }

    /**
     * 获取已经下载的发音人（本地）
     */
    public String parseSpeaker() {

        String speaker = mTts.getParameter(SpeechSynthesizer.LOCAL_SPEAKERS);

        return speaker;
    }

    public void onDestroy() {
        mTts.stopSpeaking(mTtsListener);

        mTts.destory();
    }

    public void setParameter(String speechConstant, String val) {
        mTts.setParameter(speechConstant, val);

        //mTts.setParameter(SpeechConstant.ENGINE_TYPE, "local");
        //mTts.setParameter(SpeechSynthesizer.VOICE_NAME, "xiaoyan");
        //mTts.setParameter(SpeechSynthesizer.SPEED, "50");
        //mTts.setParameter(SpeechSynthesizer.PITCH, "50");
        //mTts.setParameter(SpeechSynthesizer.VOLUME, "100");
    }

    private InitListener mTtsInitListener = new InitListener() {

        @Override
        public void onInit(ISpeechModule arg0, int code) {

            if (code == ErrorCode.SUCCESS) {
                //((Button) findViewById(R.id.tts_play)).setEnabled(true);
            }
        }
    };

    private SynthesizerListener mTtsListener = new SynthesizerListener.Stub() {
        @Override
        public void onSpeakBegin() throws RemoteException {
            //To change body of implemented methods use File | Settings | File Templates.
        }

        @Override
        public void onSpeakPaused() throws RemoteException {
            //To change body of implemented methods use File | Settings | File Templates.
        }

        @Override
        public void onSpeakResumed() throws RemoteException {
            //To change body of implemented methods use File | Settings | File Templates.
        }

        @Override
        public void onCompleted(int i) throws RemoteException {
            //To change body of implemented methods use File | Settings | File Templates.
        }

        @Override
        public void onSpeakProgress(int i) throws RemoteException {
            //To change body of implemented methods use File | Settings | File Templates.
        }

        @Override
        public void onBufferProgress(int i) throws RemoteException {
            //To change body of implemented methods use File | Settings | File Templates.
        }
    };
}
