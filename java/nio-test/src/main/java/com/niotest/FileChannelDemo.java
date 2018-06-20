package com.niotest;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.nio.ByteBuffer;
import java.nio.channels.FileChannel;

public class FileChannelDemo {

    private String root;

    public FileChannelDemo(String root) {
        this.root = root;
    }

    public void read()
            throws FileNotFoundException, IOException {

        String path = root + "data.txt";
        RandomAccessFile aFile = new RandomAccessFile(path, "rw");

        FileChannel fileChannel = aFile.getChannel();

        ByteBuffer buffer = ByteBuffer.allocate(64);

        int count = fileChannel.read(buffer);
        while (count != -1) {
            //System.out.println("Read " + count);
            buffer.flip();

            while (buffer.hasRemaining()) {
                System.out.print((char) buffer.get());
            }

            buffer.clear();
            count = fileChannel.read(buffer);
        }

        aFile.close();
        fileChannel.close();

    }

    public void transfer()
            throws IOException {

        String path = root + "copydata.txt";
        File f = new File(path);

        if(f.exists()){
            f.delete();
        }
        f.createNewFile();


        RandomAccessFile fromFile = new RandomAccessFile(root + "data.txt", "rw");
        FileChannel fromChannel = fromFile.getChannel();

        RandomAccessFile toFile = new RandomAccessFile(path, "rw");
        FileChannel toChannel = toFile.getChannel();

        long position = 0;
        long count  = fromChannel.size();

        toChannel.transferFrom(fromChannel, position, count);

        fromFile.close();
        fromChannel.close();

        toFile.close();
        toChannel.close();

    }


}
