package view;

import javax.faces.context.FacesContext;

public class RuleExecute {
    
    public RuleExecute() {
        super();
    }
    
    public void runRule(RuleResult rr){
        System.out.println("Start execute rule...");
        int c = 3;
        for(int i=0; i<c; i++){
            try{
                System.out.println((c - i) + " ...");
                Thread.sleep(1000);
            }catch(Exception e){
                e.printStackTrace();
            }
        }
        System.out.println("OK...Return result...");
        doReport("Rule_execute_successed", rr);
    }
    
    public void doReport(String result, RuleResult rr){
        rr.reportResult(result);
    }
}
