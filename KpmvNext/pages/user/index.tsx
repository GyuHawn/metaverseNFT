import useSWR from 'swr'
import Axios from "axios";
import 'bootstrap/dist/css/bootstrap.css';
import {Button, Card, Col, Row, Tab, Table} from 'react-bootstrap';
import Nav from "react-bootstrap/Nav";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUser} from "@fortawesome/free-solid-svg-icons";
import {useRouter} from "next/router";

const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

const UserList = () => {
    const rt1 = useRouter();
    const LogoutUrl = '/api/session?logout';
    const sessionData = '/api/session?data';
    const {data, error, isLoading} = useSWR(sessionData, axios1);

    if (error) {
        return <>error!</>;
    }
    if (!data) {
        return <>loading</>;
    }
    return (
        <>
            <div className="m-5">
                <Tab.Container id="left-tabs-example" defaultActiveKey="first">
                    <Row>
                        <Col sm={2}>
                            <Nav variant="pills" className="flex-column">
                                <Nav.Item>
                                    <Card>
                                        <FontAwesomeIcon className="mt-3" icon={faUser} size={"8x"}/>
                                        <Card.Body>
                                            <Card.Title>{data.userdata.name}</Card.Title>
                                            <Card.Text>
                                                MyProfile <Button className={"m-3"} onClick={() => {
                                                Axios.post(LogoutUrl, {Userid: data?.userdata?.name}).then(() => {
                                                    rt1.reload();
                                                });
                                            }}>Logout</Button>
                                            </Card.Text>
                                        </Card.Body>
                                    </Card>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link eventKey="first">나의 정보</Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link eventKey="second">나의 nft</Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link eventKey="third">우승 정보</Nav.Link>
                                </Nav.Item>
                                <Nav.Item>

                                </Nav.Item>
                            </Nav>
                        </Col>
                        <Col sm={9}>
                            <Tab.Content>
                                <Tab.Pane eventKey="first">
                                    여기에 내용
                                    추가asdfasldkfjabsldfkajsbdflaksjdfbalskdjfbasldkfjbasldkjfbaslkdjfbaslkdjfbaslkdjfbasldfkjabsdfl
                                    {/*<Sonnet />*/}
                                </Tab.Pane>
                                <Tab.Pane eventKey="second">
                                    {/*<Sonnet />*/}
                                </Tab.Pane>
                                <Tab.Pane eventKey="third">
                                    {/*<Sonnet />*/}
                                </Tab.Pane>
                            </Tab.Content>
                        </Col>
                    </Row>
                </Tab.Container>
            </div>
        </>
    )
}
export default UserList