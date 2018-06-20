package com.niotest;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.SocketChannel;

public class SocketChannelDemo {

    public void read()
            throws IOException {

        SocketChannel socketChannel = SocketChannel.open();
        socketChannel.connect(new InetSocketAddress("127.0.0.1", 8800));

        ByteBuffer buffer = ByteBuffer.allocate(256);

        int count = socketChannel.read(buffer);
        while (count != -1){

            buffer.flip();

            while (buffer.hasRemaining()){

                System.out.print((char) buffer.get());
            }

            buffer.clear();
            count = socketChannel.read(buffer);
        }

        socketChannel.close();
    }

}
