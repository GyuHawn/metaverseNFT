import useSWR from 'swr'
import Axios from "axios";
import 'bootstrap/dist/css/bootstrap.css';
import {Table} from 'react-bootstrap';

const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

const UserList = () => {
    const {data, error, isLoading} = useSWR("/api/auth", axios1);
    if (error) {
        return <>error!</>;
    }
    if (!data) {
        return <>loading</>;
    }
    return (
        <div>
            <Table>
                {data.map((e: { userid: string, username: string, EOA: string, wins: string }) => {
                    if (e.username !== "admin") {
                        return <>
                            <tr>
                                <td>회원ID : {e.userid}</td>
                                <td>회원이름 : {e.username}</td>
                                <td>NFT계정주소 : {e.EOA}</td>
                                <td>우승횟수 : {e.wins}</td>
                            </tr>
                        </>
                    }
                })}
            </Table>
        </div>
    )
}
    export default UserList