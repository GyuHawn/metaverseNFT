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
        <>
            <div className={"d-flex justify-content-center align-items-center"}>
                <div>
                    <Table striped bordered hover>
                        {data.map((e: { userid: string, username: string, EOA: string, wins: string }) => {
                            if (e.username !== "admin") {
                                return <>
                                    <tr>
                                        <th>회원ID : {e.userid}</th>
                                        <th>회원이름 : {e.username}</th>
                                        <th>NFT계정주소 : {e.EOA}</th>
                                        <th>우승횟수 : {e.wins}</th>
                                    </tr>
                                </>
                            }
                        })}
                    </Table>
                </div>
            </div>
        </>
    )
}
export default UserList