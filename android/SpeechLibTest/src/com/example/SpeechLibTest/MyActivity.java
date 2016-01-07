package com.example.SpeechLibTest;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import com.example.SpeechHelper;
import com.iflytek.speech.SpeechConstant;
import com.iflytek.speech.SpeechSynthesizer;

public class MyActivity extends Activity {
    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);

        speech = new SpeechHelper(this, "52060579");

        speech.setParameter(SpeechConstant.ENGINE_TYPE, "local");
        speech.setParameter(SpeechSynthesizer.VOICE_NAME, "xiaoyan");
        speech.setParameter(SpeechSynthesizer.SPEED, "50");
        speech.setParameter(SpeechSynthesizer.PITCH, "50");
        speech.setParameter(SpeechSynthesizer.VOLUME, "100");

        Button button = (Button) findViewById(R.id.button);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                speech.startSpeaking("123");
                String speaker = speech.parseSpeaker();
                System.out.println("yi xia zai fa yin ren" + speaker);
            }
        });

        //String speaker = speech.parseSpeaker();

        //System.out.println("yi xia zai fa yin ren" + speaker);

    }

    private SpeechHelper speech;
}
