import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;

public class FileLength {
    public void main(String[] args) {
        int count = 0;
        InputStream streamReader = null;

        if (args.length < 1) {
            System.out.println("Usage: java FileLength <filename>");
            System.exit(0);
        }

        try {

            streamReader = new FileInputStream(args[0]);
            int data = streamReader.read();
        } catch (IOException ex) {

        } finally {
            if (streamReader != null) {
                try {
                    streamReader.close();
                } catch(Exception e) {
                }
            }
        }
    }
}
