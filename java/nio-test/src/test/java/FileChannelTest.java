import com.niotest.FileChannelDemo;
import org.junit.Test;

import java.io.IOException;

public class FileChannelTest {

    @Test
    public void test()
            throws IOException {

        System.out.println("用户的当前工作目录:" + System.getProperty("user.dir"));
        String path = this.getClass().getClassLoader().getResource("").getPath();

        FileChannelDemo fileChanneldemo = new FileChannelDemo(path);
        fileChanneldemo.read();
        fileChanneldemo.transfer();
    }
}
