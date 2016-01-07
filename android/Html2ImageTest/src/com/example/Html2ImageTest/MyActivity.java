package com.example.Html2ImageTest;

import android.app.Activity;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.IBinder;
import android.os.RemoteException;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import com.iflytek.speech.*;

public class MyActivity extends Activity {

    private SpeechSynthesizer mTts;

    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);

        SpeechUtility.getUtility(this).setAppid("4d6774d0");
        mTts = new SpeechSynthesizer(this, mTtsInitListener);
        setParam();

        Button btn = (Button) findViewById(R.id.btnTest);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                int code = mTts.startSpeaking("abc", mTtsListener);
            }
        });

    }

    /**
     * 参数设置
     *
     * @param param
     * @return
     */
    private void setParam() {
        mTts.setParameter(SpeechConstant.ENGINE_TYPE, "local");
        mTts.setParameter(SpeechSynthesizer.VOICE_NAME, "xiaoyan");
        mTts.setParameter(SpeechSynthesizer.SPEED, "50");
        mTts.setParameter(SpeechSynthesizer.PITCH, "50");
        mTts.setParameter(SpeechSynthesizer.VOLUME, "100");
    }

    /**
     * 初期化监听。
     */
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
