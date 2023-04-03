import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import {useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faMeta} from "@fortawesome/free-brands-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons"
import Axios from "axios";
import {useRouter} from "next/router";
import useSWR from "swr";
import is from "@sindresorhus/is";
import dataView = is.dataView;
import {Stack} from "react-bootstrap";

const axios1 = (url: string) => Axios.get(url).then((res) => res.data)

function Header() {

    const rt1 = useRouter();
    const LogoutUrl = '/api/session?logout';
    const sessionData = '/api/session?data';
    const {data, error, isLoading} = useSWR(sessionData, axios1);
    const [ipt1, setipt1] = useState("");

    if (error) {
        return (<>error:{JSON.stringify(error)}</>);
    }
    if (!data) {
        return (<> loading data:{data}</>);
    }
    if (data?.userdata?.name === "admin") {
        return (
            <Navbar bg="light" expand="lg">
                <Container fluid>
                    <Navbar.Brand href="/"><FontAwesomeIcon icon={faMeta}/> 메타버스 퀴즈 대회</Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbarScroll"/>
                    <Navbar.Collapse id="navbarScroll">
                        <Nav
                            className="me-auto my-2 my-lg-0"
                            style={{maxHeight: '200px'}}
                            navbarScroll
                        >
                            <Nav.Link href="/">Home</Nav.Link>
                            <Nav.Link href ="/quiz">
                                퀴즈 목록
                            </Nav.Link>
                            <Nav.Link href="/object">
                                Object 목록
                            </Nav.Link>
                            <Nav.Link href="/competitionList">
                                역대 수상 목록
                            </Nav.Link>
                            <Nav.Link href="/nftusage_test">
                                SmartContract
                            </Nav.Link>
                            <Nav.Link href="/user/userlist">
                                회원 목록
                            </Nav.Link>
                            {/*<Nav.Link href="#" disabled>
                                nft 조회
                            </Nav.Link>*/}
                        </Nav>
                        <Form className="d-flex">
                            <Form.Control
                                type="search"
                                placeholder="Search"
                                value={ipt1}
                                onChange={(e) => setipt1(e.target.value)}
                                className="me-2"
                                aria-label="Search"
                            />
                            <Button variant="outline-success"
                                    href={"/user/" + ipt1}><FontAwesomeIcon
                                icon={faMagnifyingGlass}/></Button>
                        </Form>
                        <Nav>
                            {data?.logon ?
                                <div>
                                    <Button className={"m-2"} href={'/user'}> {data?.userdata?.name} 개인정보 </Button>
                                    <Button onClick={() => {
                                        Axios.post(LogoutUrl, {Userid: data?.userdata?.name}).then(() => {
                                            rt1.push('/');
                                            rt1.reload();
                                        });
                                    }}> Logout </Button>
                                </div> :
                                <div>
                                    <Stack direction="horizontal" gap={2}>
                                        <Button href="/login" className={"ms-2"} variant="secondary">SignIn</Button>
                                        <div className="vr" />
                                        <Button href="/signup" variant="outline-danger">SingUp</Button>
                                    </Stack>
                                </div>
                            }
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        );
    } else {
        return (
            <Navbar bg="light" expand="lg">
                <Container fluid>
                    <Navbar.Brand href="/"><FontAwesomeIcon icon={faMeta}/> 메타버스 퀴즈 대회</Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbarScroll"/>
                    <Navbar.Collapse id="navbarScroll">
                        <Nav
                            className="me-auto my-2 my-lg-0"
                            style={{maxHeight: '200px'}}
                            navbarScroll
                        >
                            <Nav.Link href="/">Home</Nav.Link>
                            <Nav.Link href="/competitionList">
                                역대 수상 목록
                            </Nav.Link>
                            <Nav.Link href="/nftusage_test">
                                SmartContract
                            </Nav.Link>
                        </Nav>
                        <Form className="d-flex">
                            <Form.Control
                                type="search"
                                placeholder="Search"
                                value={ipt1}
                                onChange={(e) => setipt1(e.target.value)}
                                className="me-2"
                                aria-label="Search"
                            />
                            <Button variant="outline-success"
                                    href={"/user/" + ipt1}><FontAwesomeIcon
                                icon={faMagnifyingGlass}/></Button>
                        </Form>
                        <Nav>
                            {data?.logon ?
                                <div>
                                    <Button className={"m-2"} href={'/user'}>{data?.userdata?.name} 개인정보 </Button>
                                    <Button variant={"danger"} onClick={() => {
                                        Axios.post(LogoutUrl, {Userid: data?.userdata?.name}).then(() => {
                                            rt1.push('/');
                                            rt1.reload();
                                        });
                                    }}> Logout </Button>
                                </div> :
                                <div>
                                    <Stack direction="horizontal" gap={2}>
                                        <Button href="/login" className={"ms-2"} variant="secondary">SignIn</Button>
                                        <div className="vr" />
                                        <Button href="/signup" variant="outline-danger">SingUp</Button>
                                    </Stack>
                                </div>
                            }
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        );
    }
}

export default Header;