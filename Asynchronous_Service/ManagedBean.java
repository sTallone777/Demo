package view;

import java.lang.reflect.Field;

import javax.faces.context.FacesContext;
import javax.faces.event.ActionEvent;

import javax.servlet.http.HttpSession;

import javax.xml.bind.DatatypeConverter;

import oracle.adf.view.rich.component.rich.nav.RichCommandButton;
import oracle.adf.view.rich.component.rich.output.RichOutputText;

import org.apache.http.client.config.RequestConfig;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.conn.ConnectTimeoutException;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.util.EntityUtils;
import org.apache.myfaces.trinidad.render.ExtendedRenderKitService;
import org.apache.myfaces.trinidad.util.Service;

public class ManagedBean implements RuleResult {
    private String ruleResult;
    private String controlId;
    private String sessionId;

    public ManagedBean() {
        super();
        try{
            HttpSession session = (HttpSession)FacesContext.getCurrentInstance().getExternalContext().getSession(true);
            this.sessionId = session.getId().split("!")[0];
        }catch(Exception e){
            e.printStackTrace();
        }
    }
    
    @Override
    public void reportResult(String result){
        this.ruleResult = result;
        CloseableHttpClient httpclient = null;
        CloseableHttpResponse response = null;
        String url = "http://localhost:8113/rule?ctlId=" + this.controlId + "&clientID=" + this.sessionId;
        int timeout = 10000;
        String charSet = "UTF-8";
        
        try{
            //リクエスト
            SkipHttpsUtil  skipHttpsUtil=new SkipHttpsUtil();
            httpclient =  (CloseableHttpClient)skipHttpsUtil.wrapClient();
            RequestConfig requestConfig = RequestConfig.custom()
                .setConnectionRequestTimeout(timeout)
                .setSocketTimeout(timeout)
                .setConnectTimeout(timeout).build();
            HttpGet httpGet = new HttpGet(url);  
            httpGet.setConfig(requestConfig);
            //レスポンス取得
            response = httpclient.execute(httpGet);  
            //レスポンス判断
            if(response.getStatusLine().getStatusCode() == 200){
                
            }else{
                
            }
            response.close();
        }catch(Exception e){
            e.printStackTrace();
        }
    }

    public void setRuleResult(String ruleResult) {
        this.ruleResult = ruleResult;
    }

    public String getRuleResult() {
        return ruleResult;
    }

    public void cb1_click(ActionEvent actionEvent) {
        FacesContext fc = FacesContext.getCurrentInstance();
        //start message
        this.ruleResult = "Rule processing...";
        //button2
        this.controlId = ((RichCommandButton)fc.getViewRoot().findComponent("cb2")).getClientId(fc);
        new Thread(new Runnable(){
            public void run(){
                RuleExecute re = new RuleExecute();
                re.runRule(ManagedBean.this);
            }
        }).start();
    }

    public void setSessionId(String sessionId) {
        this.sessionId = sessionId;
    }

    public String getSessionId() {
        return sessionId;
    }
}
