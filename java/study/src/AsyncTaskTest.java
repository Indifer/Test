import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Random;
import java.util.concurrent.*;
import java.util.function.Consumer;
import java.util.function.Supplier;

public class AsyncTaskTest {
    static List<Integer> list;

    static {
        list = new ArrayList<>();
        for (int i = 0; i < 10; i++) {

            list.add(new Random().nextInt(100) * 10);
        }
    }

    /**
     *
     */
    public static class CallableTest implements Callable<Integer> {

        private Integer i;

        public CallableTest(Integer i) {
            this.i = list.get(i);
        }

        @Override
        public Integer call() throws Exception {

            Thread.sleep(i);
            return i;
        }
    }

    /**
     *
     */
    public void executorServiceTest() {

        System.out.println("ExecutorServiceTest...");
        //异步列表
        List<FutureTask<Integer>> futureTasks = new ArrayList<>();

        //初始化线程池
        ExecutorService executorService = Executors.newFixedThreadPool(5);

        long startTime = System.currentTimeMillis();

        for (int i = 0; i < 10; i++) {
            Callable<Integer> callable = new CallableTest(i);
            FutureTask<Integer> task = new FutureTask<Integer>(callable);
            futureTasks.add(task);

            executorService.submit(task);
        }

        executorService.shutdown();

        for (Future<Integer> future : futureTasks) {

            try {
                System.out.println("future.get():" + future.get().toString());
            } catch (Exception ex) {
                ex.printStackTrace();
            }
        }

        long endTime = System.currentTimeMillis();
        System.out.println(Float.toString((endTime - startTime)) + " millis.");

    }

    /**
     *
     */
    public void completionServiceTest() {

        System.out.println("CompletionServiceTest...");

        //初始化线程池
        ExecutorService executorService = Executors.newFixedThreadPool(5);
        CompletionService completionService = new ExecutorCompletionService(executorService);


        long startTime = System.currentTimeMillis();
        for (int i = 0; i < 10; i++) {
            Callable<Integer> callable = new CallableTest(i);
            completionService.submit(callable);
        }

        for (int i = 0; i < 10; i++) {

            try {
                Future<Integer> future = completionService.take();
                if (future == null) break;
                System.out.println("future.get():" + future.get().toString());
            } catch (Exception ex) {
                ex.printStackTrace();
            }
        }
        long endTime = System.currentTimeMillis();
        System.out.println(Float.toString((endTime - startTime)) + " millis.");

    }

    /**
     *
     * @throws Exception
     */
    public void completableFutureTest() throws Exception {

        System.out.println("CompletableFutureTest...");
        //初始化线程池
        ExecutorService executorService = Executors.newFixedThreadPool(5);
        CompletableFuture<String> future = CompletableFuture.supplyAsync(new Supplier<String>() {
            @Override
            public String get() {
                System.out.println("task started!");
                try {
                    //模拟耗时操作
                    Thread.sleep(2000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                return "task finished!";
            }
        }, executorService);

        future.thenAccept(new Consumer<String>() {
            @Override
            public void accept(String s) {


                System.out.println(s);
            }
        });
        future.join();

    }

}
