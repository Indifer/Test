import com.niotest.SocketChannelDemo;
import org.junit.Test;

import java.io.IOException;

public class SocketChannelTest {

    @Test
    public void test()
        throws IOException{

        SocketChannelDemo socketChannelDemo = new SocketChannelDemo();
        socketChannelDemo.read();

    }

}
