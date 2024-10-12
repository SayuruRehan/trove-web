import axios from "axios";
import { BASE_URL } from "./BASEUrl";

class NotificationService {
 
    addNotification(notifyObj){
        return axios.post(`${BASE_URL}/api/notification/add`,notifyObj)
    }

}

export default new NotificationService();
